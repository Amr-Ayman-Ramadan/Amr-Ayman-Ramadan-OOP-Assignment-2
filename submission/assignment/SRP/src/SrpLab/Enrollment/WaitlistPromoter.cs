namespace SrpLab.Enrollment;

/// <summary>Operations policy: how many waiting students get promoted when seats open.</summary>
public sealed class WaitlistPromoter
{
    public int Promote(CourseRoster roster, int seats)
    {
        var promoted = 0;
        while (seats > 0 && roster.SeatNextFromWaitlist())
        {
            seats--;
            promoted++;
        }
        return promoted;
    }
}
