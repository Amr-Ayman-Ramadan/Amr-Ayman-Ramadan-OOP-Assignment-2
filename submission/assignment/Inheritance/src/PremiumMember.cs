namespace LibrarySystem;

public class PremiumMember : Member
{
    private const int PointsPerReturnedLoan = 5;

    public PremiumMember(string personId, string fullName, string phone, decimal discountPercent)
        : base(personId, fullName, phone, 10, discountPercent)
    {
    }

    // Computed every time — nobody can type a points number in.
    public int ReadingPoints
    {
        get
        {
            int points = 0;
            foreach (Loan loan in Loans)
            {
                if (loan.Status == LoanStatus.Returned)
                    points += PointsPerReturnedLoan;
            }
            return points;
        }
    }
}
