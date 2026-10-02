namespace LibrarySystem;

public class HeadLibrarian : Staff
{
    private const decimal ResponsibilityAllowance = 400m;

    public HeadLibrarian(string personId, string fullName, string phone, DateTime hireDate, decimal monthlySalary)
        : base(personId, fullName, phone, hireDate, monthlySalary, ResponsibilityAllowance)
    {
    }

    public void ChangeLateFee(LibraryItem item, decimal newBaseFee)
    {
        item.ChangeBaseLateFee(newBaseFee);
    }

    public void Withdraw(LibraryItem item)
    {
        item.Withdraw();
    }

    public void Restore(LibraryItem item)
    {
        item.Restore();
    }
}
