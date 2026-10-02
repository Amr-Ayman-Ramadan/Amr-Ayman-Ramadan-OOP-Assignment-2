namespace SrpLab.Lending;

/// <summary>Analytics export schema for underwriters.</summary>
public sealed class UnderwriterCsvExporter
{
    public string Row(string applicationId, LoanDecision decision)
    {
        var app = decision.Application;
        return $"{applicationId},{app.CreditScore},{app.EmploymentMonths},{(app.HasCollateral ? 1 : 0)},{decision.RiskScore:0.00},{(decision.IsEligible ? "Y" : "N")}";
    }
}
