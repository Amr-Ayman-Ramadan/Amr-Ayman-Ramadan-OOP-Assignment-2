namespace SrpLab.Checkout;

/// <summary>Talks to the (fake) payment gateway and returns an authorization code.</summary>
public sealed class PaymentAuthorizer
{
    public string Authorize(decimal amount, string cardLast4, int lineCount)
    {
        var payload = $"{amount:0.00}|{cardLast4}|{lineCount}";
        var hash = payload.GetHashCode();
        return $"AUTH-{Math.Abs(hash):X8}";
    }
}
