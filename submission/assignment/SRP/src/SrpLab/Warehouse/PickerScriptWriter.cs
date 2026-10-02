namespace SrpLab.Warehouse;

/// <summary>Handheld-device wording shown to the picker.</summary>
public sealed class PickerScriptWriter
{
    public string Write(IReadOnlyList<PickStop> route, IReadOnlyList<Allocation> allocations)
    {
        var steps = route.Select((s, i) => $"{i + 1}. Go aisle {s.Aisle} bin {s.Bin}: pick {s.Qty} × {s.Sku}");
        var shortfalls = allocations.Where(a => a.IsShort).ToList();
        var warn = shortfalls.Any()
            ? "SHORTAGES: " + string.Join(", ", shortfalls.Select(s => s.Sku))
            : "SHORTAGES: none";
        return string.Join('\n', steps) + "\n" + warn;
    }
}
