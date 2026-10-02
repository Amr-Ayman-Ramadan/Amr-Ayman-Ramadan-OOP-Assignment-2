namespace SrpLab.Billing;

/// <summary>Collections policy: how strong the reminder is after N failed payments.</summary>
public sealed class DunningSeverityPolicy
{
    public string SeverityFor(int failedPayments) => failedPayments switch
    {
        <= 1 => "friendly reminder",
        2 => "second notice",
        _ => "final notice before suspension"
    };
}
