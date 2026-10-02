namespace SrpLab.Enrollment;

/// <summary>Seat allocation: who is seated, who is waiting, and registering new students.</summary>
public sealed class CourseRoster
{
    private readonly HashSet<string> _seated = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _waitlist = new();

    public Course Course { get; }

    public CourseRoster(Course course) => Course = course;

    public bool HasFreeSeat => _seated.Count < Course.Capacity;
    public int WaitlistCount => _waitlist.Count;

    public string Register(string studentEmail)
    {
        if (string.IsNullOrWhiteSpace(studentEmail)) throw new ArgumentException("email");
        var email = studentEmail.Trim();

        if (_seated.Contains(email) || WaitlistPosition(email) > 0)
            return "ALREADY_REGISTERED";

        if (HasFreeSeat)
        {
            _seated.Add(email);
            return "SEATED";
        }

        _waitlist.Add(email);
        return $"WAITLIST:{_waitlist.Count}";
    }

    public bool IsSeated(string studentEmail) => _seated.Contains(studentEmail);

    public int WaitlistPosition(string studentEmail)
    {
        var idx = _waitlist.FindIndex(x => x.Equals(studentEmail, StringComparison.OrdinalIgnoreCase));
        return idx < 0 ? -1 : idx + 1;
    }

    /// <summary>Moves the first waiting student into a seat (if there is one).</summary>
    public bool SeatNextFromWaitlist()
    {
        if (_waitlist.Count == 0 || !HasFreeSeat) return false;
        var next = _waitlist[0];
        _waitlist.RemoveAt(0);
        _seated.Add(next);
        return true;
    }
}
