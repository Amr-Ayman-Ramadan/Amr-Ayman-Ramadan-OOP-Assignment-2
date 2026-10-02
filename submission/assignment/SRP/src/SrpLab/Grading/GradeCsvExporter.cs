namespace SrpLab.Grading;

/// <summary>CSV export layout of all student standings.</summary>
public sealed class GradeCsvExporter
{
    public string Export(IEnumerable<StudentStanding> standings)
    {
        var rows = new List<string> { "studentId,average,letter,honor" };
        foreach (var s in standings)
            rows.Add($"{s.StudentId},{s.Average},{s.Letter},{(s.HonorRoll ? 1 : 0)}");
        return string.Join('\n', rows);
    }
}
