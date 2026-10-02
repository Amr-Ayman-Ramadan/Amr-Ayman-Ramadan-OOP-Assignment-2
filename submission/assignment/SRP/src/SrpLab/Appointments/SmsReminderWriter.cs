namespace SrpLab.Appointments;

/// <summary>SMS channel wording for appointment reminders.</summary>
public sealed class SmsReminderWriter
{
    public string Write(DateTimeOffset slot, string clinicPhone) =>
        $"Reminder: appointment {slot:MMM dd HH:mm}. Call {clinicPhone} to reschedule.";
}
