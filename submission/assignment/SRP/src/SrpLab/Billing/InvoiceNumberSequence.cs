namespace SrpLab.Billing;

/// <summary>Invoice numbering scheme. Calling Next() is the ONLY way a number is consumed.</summary>
public sealed class InvoiceNumberSequence
{
    private int _last;
    private readonly object _lock = new();

    public InvoiceNumberSequence(int lastUsed = 1000) => _last = lastUsed;

    public string Next(DateOnly periodStart)
    {
        lock (_lock)
        {
            _last++;
            return $"INV-{periodStart:yyyyMM}-{_last:D5}";
        }
    }
}
