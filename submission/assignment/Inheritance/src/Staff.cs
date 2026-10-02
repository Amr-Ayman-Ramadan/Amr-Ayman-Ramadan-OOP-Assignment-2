namespace LibrarySystem;

public class Staff : Person
{
    private readonly decimal _monthlyAllowance;

    public DateTime HireDate { get; }
    public decimal MonthlySalary { get; private set; }

    protected Staff(string personId, string fullName, string phone, DateTime hireDate, decimal monthlySalary, decimal monthlyAllowance)
        : base(personId, fullName, phone)
    {
        if (monthlySalary <= 0)
            throw new ArgumentException("Monthly salary must be greater than zero.");
        if (monthlyAllowance < 0)
            throw new ArgumentException("Allowance cannot be negative.");

        HireDate = hireDate;
        MonthlySalary = monthlySalary;
        _monthlyAllowance = monthlyAllowance;
    }

    public void GiveRaise(decimal percent)
    {
        if (percent <= 0)
            throw new ArgumentException("A raise must be a positive percentage.");

        MonthlySalary = MonthlySalary + MonthlySalary * percent / 100m;
    }

    // One method for every kind of staff: the child only passed its allowance through base(...).
    public decimal GetMonthlyPay()
    {
        return MonthlySalary + _monthlyAllowance;
    }
}
