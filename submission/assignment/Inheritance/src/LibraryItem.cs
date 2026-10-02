namespace LibrarySystem;

// Never "just an item": protected constructor, only Book / Dvd / Magazine can be created.
public class LibraryItem
{
    private readonly decimal _lateFeeMultiplier;

    public string CatalogNumber { get; }
    public string Title { get; }
    public int LoanPeriodDays { get; }
    public decimal BaseLateFee { get; private set; }
    public bool IsWithdrawn { get; private set; }
    public bool IsOnLoan { get; private set; }

    protected LibraryItem(string catalogNumber, string title, decimal baseLateFee, int loanPeriodDays, decimal lateFeeMultiplier)
    {
        if (string.IsNullOrWhiteSpace(catalogNumber))
            throw new ArgumentException("Catalog number cannot be empty.");
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");
        if (baseLateFee <= 0)
            throw new ArgumentException("Base late fee must be greater than zero.");

        CatalogNumber = catalogNumber;
        Title = title;
        BaseLateFee = baseLateFee;
        LoanPeriodDays = loanPeriodDays;
        _lateFeeMultiplier = lateFeeMultiplier;
    }

    // Book x1, DVD x2, Magazine x0.5 — the child passed its multiplier, no type checks needed.
    public decimal DailyLateFee
    {
        get { return BaseLateFee * _lateFeeMultiplier; }
    }

    internal void ChangeBaseLateFee(decimal newFee)
    {
        if (newFee <= 0)
            throw new ArgumentException("A late fee must be greater than zero.");
        BaseLateFee = newFee;
    }

    internal void Withdraw()
    {
        IsWithdrawn = true;
    }

    internal void Restore()
    {
        IsWithdrawn = false;
    }

    // Called only by Loan when it is created.
    internal void MarkBorrowed()
    {
        if (IsWithdrawn)
            throw new InvalidOperationException($"'{Title}' is withdrawn from circulation and cannot be borrowed.");
        if (IsOnLoan)
            throw new InvalidOperationException($"'{Title}' is already on loan to someone else.");
        IsOnLoan = true;
    }

    // Called only by Loan when it is returned.
    internal void MarkReturned()
    {
        IsOnLoan = false;
    }
}
