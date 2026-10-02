namespace SrpLab.Warehouse;

/// <summary>The list of lines that have to be picked for an order batch.</summary>
public sealed class WarehousePickList
{
    private readonly List<PickLine> _lines = new();

    public IReadOnlyList<PickLine> Lines => _lines;

    public void AddNeed(string sku, string aisle, int bin, int qtyNeeded, int qtyOnHand) =>
        _lines.Add(new PickLine(sku, aisle, bin, qtyNeeded, qtyOnHand));
}
