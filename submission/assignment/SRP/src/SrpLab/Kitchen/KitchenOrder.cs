namespace SrpLab.Kitchen;

/// <summary>The dishes on one order.</summary>
public sealed class KitchenOrder
{
    private readonly List<KitchenOrderItem> _items = new();

    public IReadOnlyList<KitchenOrderItem> Items => _items;

    public void AddItem(string item, IEnumerable<string> ingredients, int prepMinutes)
    {
        var cleaned = ingredients.Select(i => i.Trim().ToLowerInvariant()).ToList();
        _items.Add(new KitchenOrderItem(item, cleaned, prepMinutes));
    }
}
