namespace SrpLab.Warehouse;

/// <summary>Allocation / backorder policy: how much of each line we can actually ship.</summary>
public sealed class StockAllocator
{
    public IReadOnlyList<Allocation> Allocate(WarehousePickList list) =>
        list.Lines.Select(l => new Allocation(l.Sku, l.QtyNeeded, Math.Min(l.QtyNeeded, l.QtyOnHand))).ToList();
}
