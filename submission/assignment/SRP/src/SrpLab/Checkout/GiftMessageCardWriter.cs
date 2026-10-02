namespace SrpLab.Checkout;

/// <summary>Writes the customer-facing gift card text.</summary>
public sealed class GiftMessageCardWriter
{
    public string Write(CheckoutBasket basket, decimal grandTotal, string fromName)
    {
        var items = string.Join(", ", basket.Lines.Select(l => l.Sku));
        return $"Dear friend,\nA gift from {fromName} awaits ({items}).\nTotal surprise value: {grandTotal:C}\n";
    }
}
