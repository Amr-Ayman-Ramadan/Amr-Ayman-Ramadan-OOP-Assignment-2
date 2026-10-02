namespace SrpLab.Ward;

/// <summary>Keeps track of which patient is in which bed and their current acuity.</summary>
public sealed class WardBoard
{
    private readonly Dictionary<int, BedOccupant> _beds = new();

    public BedOccupant AssignBed(int bed, string patientId, int acuity)
    {
        if (bed <= 0) throw new ArgumentOutOfRangeException(nameof(bed));
        if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentException("patient required");

        var occupant = new BedOccupant(bed, patientId.Trim().ToUpperInvariant(), acuity);
        _beds[bed] = occupant;
        return occupant;
    }

    public BedOccupant? FindOccupant(int bed) => _beds.TryGetValue(bed, out var o) ? o : null;

    public IReadOnlyList<BedOccupant> OccupiedBeds() => _beds.Values.OrderBy(o => o.Bed).ToList();
}
