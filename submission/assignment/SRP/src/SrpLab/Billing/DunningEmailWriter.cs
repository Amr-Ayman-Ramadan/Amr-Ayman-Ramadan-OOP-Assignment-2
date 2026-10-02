namespace SrpLab.Billing;

/// <summary>Email copy for payment reminders. Pure formatting — it does not mint invoice numbers.</summary>
public sealed class DunningEmailWriter
{
    private readonly DunningSeverityPolicy _severity;

    public DunningEmailWriter(DunningSeverityPolicy severity) => _severity = severity;

    public string Write(Subscription sub, string customerName, DateOnly asOf, decimal balance, string invoiceNumber)
    {
        var severity = _severity.SeverityFor(sub.FailedPayments);
        return $"Subject: {severity} {invoiceNumber}\nHi {customerName},\nBalance {balance:C} as of {asOf:o} ({sub.FailedPayments} failures).\n";
    }
}
