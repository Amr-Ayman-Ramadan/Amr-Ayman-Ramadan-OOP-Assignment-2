namespace SrpLab.Lending;

/// <summary>Underwriting workflow: runs risk, eligibility and checklist and returns one decision.</summary>
public sealed class LoanDesk
{
    private readonly LoanRiskModel _riskModel;
    private readonly LoanEligibilityPolicy _eligibility;
    private readonly RequiredDocumentsChecklist _checklist;

    public LoanDesk(LoanRiskModel riskModel, LoanEligibilityPolicy eligibility, RequiredDocumentsChecklist checklist)
    {
        _riskModel = riskModel;
        _eligibility = eligibility;
        _checklist = checklist;
    }

    public LoanDecision Evaluate(LoanApplication app)
    {
        var eligible = _eligibility.IsEligible(app);
        return new LoanDecision(app, _riskModel.RiskScore(app), eligible, _checklist.For(app, eligible));
    }
}
