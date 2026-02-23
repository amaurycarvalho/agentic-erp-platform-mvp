namespace Agent.Application.UseCases.ProcessAgentCommand;

public sealed record AgentCommandRequest(
    string Message,
    string? CustomerId,
    decimal? TotalAmount,
    string? InvoiceId,
    string? Reason);
