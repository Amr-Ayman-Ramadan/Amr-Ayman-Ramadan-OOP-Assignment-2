namespace SrpLab.Lending;

/// <summary>The facts the applicant gave us.</summary>
public sealed record LoanApplication(decimal RequestedAmount, int CreditScore, int EmploymentMonths, bool HasCollateral);
