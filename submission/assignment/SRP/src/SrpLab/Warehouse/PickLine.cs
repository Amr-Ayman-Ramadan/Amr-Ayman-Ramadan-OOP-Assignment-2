namespace SrpLab.Warehouse;

public sealed record PickLine(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand);

public sealed record Allocation(string Sku, int Needed, int Allocated)
{
    public bool IsShort => Allocated < Needed;
}

public sealed record PickStop(string Aisle, int Bin, string Sku, int Qty);
