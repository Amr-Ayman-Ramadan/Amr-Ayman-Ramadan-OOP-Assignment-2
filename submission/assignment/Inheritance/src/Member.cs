namespace LibrarySystem;

public class Member : Person
{
    private readonly List<Loan> _loans = new List<Loan>();

    // Outside code can read the history but cannot Add / Remove through it.
    public IReadOnlyList<Loan> Loans
    {
        get { return _loans.AsReadOnly(); }
    }

    public int MaxActiveLoans { get; }
    public decimal DiscountPercent { get; }

    protected Member(string personId, string fullName, string phone, int maxActiveLoans, decimal discountPercent)
        : base(personId, fullName, phone)
    {
        if (maxActiveLoans <= 0)
            throw new ArgumentException("Max active loans must be positive.");
        if (discountPercent < 0 || discountPercent > 100)
            throw new ArgumentException("Discount must be between 0 and 100 percent.");

        MaxActiveLoans = maxActiveLoans;
        DiscountPercent = discountPercent;
    }

    public int CountActiveLoans()
    {
        int count = 0;
        foreach (Loan loan in _loans)
        {
            if (loan.Status == LoanStatus.Borrowed)
                count++;
        }
        return count;
    }

    // The only way a loan is added to the history: the member's own borrow action.
    public Loan Borrow(string loanId, LibraryItem item, DateTime borrowDate)
    {
        if (CountActiveLoans() >= MaxActiveLoans)
            throw new InvalidOperationException(
                $"{FullName} already has {MaxActiveLoans} active loans (the maximum) and cannot borrow '{item.Title}'.");

        // The Loan constructor asks the item to check itself (withdrawn / already on loan).
        Loan loan = new Loan(loanId, borrowDate, this, item);
        _loans.Add(loan);
        return loan;
    }
}
