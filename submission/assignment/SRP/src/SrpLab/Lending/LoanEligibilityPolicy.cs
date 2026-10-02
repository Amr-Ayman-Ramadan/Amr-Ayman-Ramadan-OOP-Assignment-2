namespace SrpLab.Lending;

/// <summary>Lending policy: the cut-offs that decide approve / reject.</summary>
public sealed class LoanEligibilityPolicy
{
    private readonly LoanRiskModel _riskModel;

    public LoanEligibilityPolicy(LoanRiskModel riskModel) => _riskModel = riskModel;

    public bool IsEligible(LoanApplication app) => _riskModel.RiskScore(app) >= 55m && app.CreditScore >= 580;
}
