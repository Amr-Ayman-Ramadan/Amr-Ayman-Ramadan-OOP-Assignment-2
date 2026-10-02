namespace SrpLab.Lending;

/// <summary>Result of evaluating an application (no wording, just the facts).</summary>
public sealed record LoanDecision(LoanApplication Application, decimal RiskScore, bool IsEligible, IReadOnlyList<string> RequiredDocuments);
