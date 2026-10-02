namespace SrpLab.Checkout;

/// <summary>Packaging policy: how much gift wrapping costs.</summary>
public sealed class GiftWrapPolicy
{
    public decimal Fee { get; } = 4.99m;

    public decimal FeeFor(bool giftWrap) => giftWrap ? Fee : 0m;
}
