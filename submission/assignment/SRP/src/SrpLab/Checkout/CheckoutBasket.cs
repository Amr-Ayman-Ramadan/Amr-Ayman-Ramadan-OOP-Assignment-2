namespace SrpLab.Checkout;

/// <summary>Holds what the customer put in the basket and the choices they made (coupon, gift wrap).</summary>
public sealed class CheckoutBasket
{
    private readonly List<BasketLine> _lines = new();

    public IReadOnlyList<BasketLine> Lines => _lines;
    public string? CouponText { get; private set; }
    public bool GiftWrap { get; private set; }

    public void AddLine(string sku, decimal price, int qty)
    {
        if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
        _lines.Add(new BasketLine(sku, price, qty));
    }

    public void ApplyCouponText(string? couponText) => CouponText = couponText;
    public void EnableGiftWrap() => GiftWrap = true;

    public decimal SubTotal() => _lines.Sum(l => l.Price * l.Qty);
}
