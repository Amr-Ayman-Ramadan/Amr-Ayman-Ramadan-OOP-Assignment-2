namespace SrpLab.Appointments;

/// <summary>Remembers which slots are taken and books new ones.</summary>
public sealed class AppointmentBook
{
    private readonly HashSet<DateTimeOffset> _booked = new();
    private readonly ClinicHours _hours;

    public AppointmentBook(ClinicHours hours) => _hours = hours;

    public bool IsBooked(DateTimeOffset slot) => _booked.Contains(slot);

    public bool IsAvailable(DateTimeOffset slot) => _hours.IsWithinBusinessHours(slot) && !IsBooked(slot);

    public bool TryBook(DateTimeOffset slot)
    {
        if (!IsAvailable(slot)) return false;
        _booked.Add(slot);
        return true;
    }
}
