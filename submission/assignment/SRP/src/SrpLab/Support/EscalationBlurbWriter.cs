namespace SrpLab.Support;

/// <summary>Internal message format used when escalating a ticket.</summary>
public sealed class EscalationBlurbWriter
{
    public string Write(SupportTicket ticket, DateTimeOffset slaDeadline) =>
        $"ESCALATE {ticket.Id} priority={ticket.Priority} breachAt={slaDeadline:u} keywords-scanned=yes";
}
