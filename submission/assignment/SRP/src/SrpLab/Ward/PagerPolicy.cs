namespace SrpLab.Ward;

/// <summary>Hospital paging policy: decides when an acuity score must page the team.</summary>
public sealed class PagerPolicy
{
    public int CodeYellowThreshold { get; } = 8;

    public bool ShouldPage(int acuity) => acuity >= CodeYellowThreshold;
}
