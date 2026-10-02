namespace SrpLab.Support;

/// <summary>Workflow for receiving customer text: opens tickets and re-prioritizes them when customers write again.</summary>
public sealed class TicketIntake
{
    private readonly TicketPriorityClassifier _classifier;

    public TicketIntake(TicketPriorityClassifier classifier) => _classifier = classifier;

    public SupportTicket Open(string id, string subject, string body, DateTimeOffset openedAt)
    {
        var ticket = new SupportTicket(id, subject, body, openedAt);
        Reprioritize(ticket);
        return ticket;
    }

    public void AddCustomerMessage(SupportTicket ticket, string text)
    {
        ticket.AppendCustomerMessage(text);
        Reprioritize(ticket);
    }

    private void Reprioritize(SupportTicket ticket) =>
        ticket.ChangePriority(_classifier.Classify(ticket.Subject, ticket.Body));
}
