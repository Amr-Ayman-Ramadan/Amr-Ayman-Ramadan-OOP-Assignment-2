namespace SrpLab.Checkout;

/// <summary>Marketing rule: reads a coupon text and works out how much it takes off the subtotal.</summary>
public sealed class CouponDiscountCalculator
{
    public decimal DiscountFor(string? couponText, decimal subTotal)
    {
        if (string.IsNullOrWhiteSpace(couponText)) return 0m;
        var t = couponText.Trim().ToUpperInvariant();
        if (t.StartsWith("SAVE") && int.TryParse(t[4..], out var pct) && pct is > 0 and <= 50)
            return Math.Round(subTotal * pct / 100m, 2);
        if (t.Contains("FREESHIP")) return 0m; // shipping discount is handled elsewhere
        if (t == "WELCOME10") return Math.Min(10m, subTotal);
        return 0m;
    }
}
