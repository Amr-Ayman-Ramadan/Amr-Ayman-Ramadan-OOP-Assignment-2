namespace SrpLab.Kitchen;

/// <summary>Printer layout: renders the order for a 32-column thermal printer.</summary>
public sealed class ThermalTicketPrinter
{
    private const int Width = 32;

    public string Render(KitchenOrder order, int orderNumber, int etaMinutes, IReadOnlyList<string> allergens)
    {
        var line = new string('=', Width);
        var body = string.Join('\n', order.Items.Select(i => $"* {i.Name.ToUpperInvariant()} ({i.PrepMinutes}m)"));
        var allergyLine = allergens.Count == 0 ? "ALLERGENS: none" : "ALLERGENS: " + string.Join(",", allergens);
        return $"{line}\nORDER #{orderNumber}\nETA {etaMinutes} MIN\n{body}\n{allergyLine}\n{line}\n";
    }
}
