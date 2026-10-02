namespace SrpLab.Enrollment;

/// <summary>Tax rule: VAT rate and VAT amount.</summary>
public sealed class VatCalculator
{
    public decimal Rate { get; } = 0.14m;

    public decimal VatOn(decimal amount) => Math.Round(amount * Rate, 2);
}
