namespace SrpLab.Billing;

/// <summary>Finance calendar rule: how much of the monthly price is due from a given day.</summary>
public sealed class ProrationCalculator
{
    public decimal Prorate(Subscription sub, DateOnly activeFrom)
    {
        if (activeFrom <= sub.PeriodStart) return sub.MonthlyPrice;
        if (activeFrom >= sub.PeriodEnd) return 0m;
        var totalDays = sub.PeriodEnd.DayNumber - sub.PeriodStart.DayNumber;
        if (totalDays <= 0) return sub.MonthlyPrice;
        var used = sub.PeriodEnd.DayNumber - activeFrom.DayNumber;
        return Math.Round(sub.MonthlyPrice * used / totalDays, 2);
    }
}
