namespace SrpLab.Billing;

/// <summary>Accounting export format of one journal line.</summary>
public sealed class LedgerJournalExporter
{
    public string Line(Subscription sub, string invoiceNumber, decimal amount) =>
        $"{sub.CustomerId},{invoiceNumber},{amount:0.00},AR-SUB";
}
