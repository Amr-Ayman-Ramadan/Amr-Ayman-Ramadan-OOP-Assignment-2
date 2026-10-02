namespace SrpLab.Appointments;

/// <summary>Search algorithm: finds the next free slot within a time window.</summary>
public sealed class SlotFinder
{
    private readonly ClinicHours _hours;
    private readonly AppointmentBook _book;

    public SlotFinder(ClinicHours hours, AppointmentBook book)
    {
        _hours = hours;
        _book = book;
    }

    public DateTimeOffset? FindNextSlot(DateTimeOffset from, int searchHours)
    {
        var cursor = _hours.AlignToSlot(from);
        var end = from.AddHours(searchHours);
        while (cursor < end)
        {
            if (_book.IsAvailable(cursor)) return cursor;
            cursor = cursor.AddMinutes(_hours.SlotMinutes);
        }
        return null;
    }
}
