namespace SrpLab.Kitchen;

/// <summary>Kitchen operations model: estimates when the order will be ready.</summary>
public sealed class PrepTimeEstimator
{
    private const int AllergyProtocolMinutes = 3;

    public int EstimateReadyMinutes(KitchenOrder order, int openStations, bool hasAllergens)
    {
        if (openStations <= 0) openStations = 1;
        var sequential = order.Items.Sum(i => i.PrepMinutes);
        var parallel = (int)Math.Ceiling(sequential / (double)openStations);
        if (hasAllergens) parallel += AllergyProtocolMinutes;
        var longest = order.Items.Count == 0 ? 0 : order.Items.Max(i => i.PrepMinutes);
        return Math.Max(parallel, longest);
    }
}
