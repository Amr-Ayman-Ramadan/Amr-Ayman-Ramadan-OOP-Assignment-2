namespace SrpLab.Warehouse;

/// <summary>Integration contract with the WMS (XML batch).</summary>
public sealed class WmsBatchXmlExporter
{
    public string Export(string batchId, IReadOnlyList<Allocation> allocations)
    {
        var parts = allocations.Select(a => $"<line sku=\"{a.Sku}\" qty=\"{a.Allocated}\" />");
        return $"<batch id=\"{batchId}\">{string.Join("", parts)}</batch>";
    }
}
