namespace SrpLab.Support;

/// <summary>Operations SLA: how long each priority may wait for a response.</summary>
public sealed class SlaPolicy
{
    public DateTimeOffset Deadline(SupportTicket ticket)
    {
        var hours = ticket.Priority switch
        {
            "P1" => 4,
            "P2" => 24,
            _ => 72
        };
        return ticket.OpenedAt.AddHours(hours);
    }

    public bool IsBreached(SupportTicket ticket, DateTimeOffset now) => now > Deadline(ticket);
}
