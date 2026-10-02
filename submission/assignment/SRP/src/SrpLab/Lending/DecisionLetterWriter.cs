namespace SrpLab.Lending;

/// <summary>Legal / communications wording of the letter sent to the applicant.</summary>
public sealed class DecisionLetterWriter
{
    public string Write(LoanDecision decision, string applicantName)
    {
        var amount = decision.Application.RequestedAmount;
        if (decision.IsEligible)
        {
            return $"Dear {applicantName},\nYour request for {amount:C} is pre-approved (risk {decision.RiskScore:0}).\n" +
                   $"Please upload: {string.Join("; ", decision.RequiredDocuments)}.\n";
        }

        return $"Dear {applicantName},\nWe are unable to approve {amount:C} at this time.\n" +
               $"Reference risk={decision.RiskScore:0}. You may reapply after improving documentation.\n";
    }
}
