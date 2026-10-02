namespace PatternsLab.Problems.Builder;

public enum AccessMode
{
    LiveGroup,
    VideosOnly
}

/// <summary>
/// The product. It can only be created through CourseRegistration.Builder (private constructor).
/// </summary>
public sealed class CourseRegistration
{
    public string StudentEmail { get; }
    public string CourseCode { get; }
    public AccessMode AccessMode { get; }
    public string? GroupCode { get; }
    public string? DiscountCode { get; }
    public bool SendWhatsApp { get; }
    public bool SendEmailWelcome { get; }
    public string? MentorNote { get; }
    public DateOnly? PreferredStart { get; }

    private CourseRegistration(Builder b)
    {
        StudentEmail = b.StudentEmail;
        CourseCode = b.CourseCode;
        AccessMode = b.Mode;
        GroupCode = b.Group;
        DiscountCode = b.Discount;
        SendWhatsApp = b.WhatsApp;
        SendEmailWelcome = b.EmailWelcome;
        MentorNote = b.Note;
        PreferredStart = b.Start;
    }

    public override string ToString()
        => $"{StudentEmail} → {CourseCode} [{AccessMode}] group={GroupCode ?? "-"} discount={DiscountCode ?? "-"} wa={SendWhatsApp} mail={SendEmailWelcome}";

    /// <summary>
    /// Fluent builder.
    /// REQUIRED: student email, course code and access mode -> constructor parameters (you can't forget them).
    /// OPTIONAL: everything else -> chainable With.../Send... methods.
    /// Build() checks the rules and throws a clear error if the combination is invalid.
    /// </summary>
    public sealed class Builder
    {
        internal string StudentEmail { get; }
        internal string CourseCode { get; }
        internal AccessMode Mode { get; }
        internal string? Group { get; private set; }
        internal string? Discount { get; private set; }
        internal bool WhatsApp { get; private set; }
        internal bool EmailWelcome { get; private set; }
        internal string? Note { get; private set; }
        internal DateOnly? Start { get; private set; }

        public Builder(string studentEmail, string courseCode, AccessMode accessMode)
        {
            StudentEmail = studentEmail;
            CourseCode = courseCode;
            Mode = accessMode;
        }

        public Builder InGroup(string groupCode) { Group = groupCode; return this; }
        public Builder WithDiscount(string discountCode) { Discount = discountCode; return this; }
        public Builder SendWhatsAppMessages() { WhatsApp = true; return this; }
        public Builder SendWelcomeEmail() { EmailWelcome = true; return this; }
        public Builder WithMentorNote(string note) { Note = note; return this; }
        public Builder PreferStartOn(DateOnly date) { Start = date; return this; }

        public CourseRegistration Build()
        {
            if (string.IsNullOrWhiteSpace(StudentEmail))
                throw new InvalidOperationException("Student email is required.");
            if (string.IsNullOrWhiteSpace(CourseCode))
                throw new InvalidOperationException("Course code is required.");
            if (Mode == AccessMode.LiveGroup && string.IsNullOrWhiteSpace(Group))
                throw new InvalidOperationException("A LiveGroup registration needs a GroupCode — call InGroup(...).");
            if (Mode == AccessMode.VideosOnly && !string.IsNullOrWhiteSpace(Group))
                throw new InvalidOperationException($"A VideosOnly registration cannot have a GroupCode (got '{Group}').");

            return new CourseRegistration(this);
        }
    }
}

public static class RegistrationCallSites
{
    public static CourseRegistration CreateLiveStudent()
    {
        return new CourseRegistration.Builder("sara@mail.com", "SEF-101", AccessMode.LiveGroup)
            .InGroup("G1")
            .WithDiscount("EARLY10")
            .SendWhatsAppMessages()
            .SendWelcomeEmail()
            .WithMentorNote("Needs evening slot")
            .PreferStartOn(new DateOnly(2026, 10, 1))
            .Build();
    }

    public static CourseRegistration CreateVideosOnly()
    {
        return new CourseRegistration.Builder("ali@mail.com", "SEF-101", AccessMode.VideosOnly)
            .SendWelcomeEmail()
            .Build();
    }
}
