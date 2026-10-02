namespace SrpLab.Checkout;

/// <summary>Combines subtotal, discount and fees into the amount the customer pays.</summary>
public sealed class CheckoutTotalCalculator
{
    private readonly CouponDiscountCalculator _discounts;
    private readonly GiftWrapPolicy _giftWrap;

    public CheckoutTotalCalculator(CouponDiscountCalculator discounts, GiftWrapPolicy giftWrap)
    {
        _discounts = discounts;
        _giftWrap = giftWrap;
    }

    public decimal GrandTotal(CheckoutBasket basket)
    {
        var subTotal = basket.SubTotal();
        var total = subTotal - _discounts.DiscountFor(basket.CouponText, subTotal);
        total += _giftWrap.FeeFor(basket.GiftWrap);
        return Math.Max(0m, total);
    }
}
