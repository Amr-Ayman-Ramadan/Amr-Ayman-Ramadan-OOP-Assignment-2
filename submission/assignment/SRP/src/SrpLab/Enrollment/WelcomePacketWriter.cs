namespace SrpLab.Enrollment;

/// <summary>Marketing content of the welcome packet (markdown).</summary>
public sealed class WelcomePacketWriter
{
    public string Write(CourseRoster roster, string studentEmail, string studentName)
    {
        var code = roster.Course.Code;
        var status = roster.IsSeated(studentEmail) ? "confirmed seat" : $"waitlist #{roster.WaitlistPosition(studentEmail)}";
        return $"# Welcome to {code}\nHi {studentName},\nYour status: **{status}**.\n" +
               $"Bring a laptop. Discord onboarding link: https://example.invalid/{code.ToLowerInvariant()}\n";
    }
}
