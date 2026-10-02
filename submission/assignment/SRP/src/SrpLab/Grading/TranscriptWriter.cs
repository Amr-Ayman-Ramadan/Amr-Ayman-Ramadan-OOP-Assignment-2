namespace SrpLab.Grading;

/// <summary>Registrar document format (plain-text transcript).</summary>
public sealed class TranscriptWriter
{
    public string Write(StudentStanding s, string fullName) =>
        $"TRANSCRIPT\nStudent: {fullName} ({s.StudentId})\nAverage: {s.Average}\nLetter: {s.Letter}\nHonor: {s.HonorRoll}\n";
}
