namespace SrpLab.Warehouse;

/// <summary>Walking-path heuristic: the order the picker visits the bins.</summary>
public sealed class PickPathPlanner
{
    public IReadOnlyList<PickStop> WalkingOrder(WarehousePickList list) =>
        list.Lines
            .OrderBy(l => l.Aisle)
            .ThenBy(l => l.Bin)
            .Select(l => new PickStop(l.Aisle, l.Bin, l.Sku, Math.Min(l.QtyNeeded, l.QtyOnHand)))
            .Where(s => s.Qty > 0)
            .ToList();
}
