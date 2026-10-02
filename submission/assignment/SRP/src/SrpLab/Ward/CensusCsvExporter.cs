namespace SrpLab.Ward;

/// <summary>Exports the ward census in CSV format.</summary>
public sealed class CensusCsvExporter
{
    public string Export(WardBoard board)
    {
        var lines = new List<string> { "bed,patient,acuity" };
        foreach (var o in board.OccupiedBeds())
            lines.Add($"{o.Bed},{o.PatientId},{o.Acuity}");
        return string.Join('\n', lines);
    }
}
