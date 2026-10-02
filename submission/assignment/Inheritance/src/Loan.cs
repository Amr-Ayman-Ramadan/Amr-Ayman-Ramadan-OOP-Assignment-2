namespace LibrarySystem;

public class Loan
{
    public string LoanId { get; }
    public DateTime BorrowDate { get; }
    public Member Member { get; }
    public LibraryItem Item { get; }          // reference to the real item, not a copy
    public LoanStatus Status { get; private set; }
    public DateTime? ReturnDate { get; private set; }

    // internal: only Member.Borrow creates loans.
    internal Loan(string loanId, DateTime borrowDate, Member member, LibraryItem item)
    {
        if (string.IsNullOrWhiteSpace(loanId))
            throw new ArgumentException("Loan ID cannot be empty.");
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        item.MarkBorrowed(); // throws if withdrawn or already on loan -> the loan is never created

        LoanId = loanId;
        BorrowDate = borrowDate;
        Member = member;
        Item = item;
        Status = LoanStatus.Borrowed;
    }

    public DateTime DueDate
    {
        get { return BorrowDate.AddDays(Item.LoanPeriodDays); }
    }

    // days late x daily fee, minus the member's discount. Zero if on time or not returned yet.
    public decimal LateFee
    {
        get
        {
            if (ReturnDate == null)
                return 0m;

            int daysLate = (ReturnDate.Value.Date - DueDate.Date).Days;
            if (daysLate <= 0)
                return 0m;

            decimal fee = daysLate * Item.DailyLateFee;
            decimal discount = fee * Member.DiscountPercent / 100m;
            return fee - discount;
        }
    }

    internal void Return(DateTime returnDate)
    {
        if (Status == LoanStatus.Returned)
            throw new InvalidOperationException($"Loan {LoanId} was already returned.");
        if (Status == LoanStatus.Lost)
            throw new InvalidOperationException($"Loan {LoanId} is marked as lost and cannot be returned.");
        if (returnDate < BorrowDate)
            throw new ArgumentException($"Return date {returnDate:yyyy-MM-dd} is before the borrow date {BorrowDate:yyyy-MM-dd}.");

        ReturnDate = returnDate;
        Status = LoanStatus.Returned;
        Item.MarkReturned();
    }

    internal void MarkAsLost()
    {
        if (Status == LoanStatus.Returned)
            throw new InvalidOperationException($"Loan {LoanId} was already returned, it cannot be marked as lost.");
        if (Status == LoanStatus.Lost)
            throw new InvalidOperationException($"Loan {LoanId} is already marked as lost.");

        Status = LoanStatus.Lost;
    }
}
