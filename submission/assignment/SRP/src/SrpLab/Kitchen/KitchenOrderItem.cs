namespace SrpLab.Kitchen;

public sealed record KitchenOrderItem(string Name, IReadOnlyList<string> Ingredients, int PrepMinutes);
