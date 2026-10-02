namespace SrpLab.Ward;

/// <summary>Collects pager messages until someone drains them.</summary>
public sealed class PagerLog
{
    private readonly List<string> _messages = new();

    public void RecordCodeYellow(int bed, DateTime atUtc) =>
        _messages.Add($"CODE-YELLOW bed={bed} at {atUtc:HH:mm}");

    public IReadOnlyList<string> Drain()
    {
        var copy = _messages.ToList();
        _messages.Clear();
        return copy;
    }
}
