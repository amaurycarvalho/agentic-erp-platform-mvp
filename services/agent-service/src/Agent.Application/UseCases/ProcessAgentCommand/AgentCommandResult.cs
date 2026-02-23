namespace Agent.Application.UseCases.ProcessAgentCommand;

public sealed record AgentCommandResult(
    string Intent,
    string Tool,
    object Output);
