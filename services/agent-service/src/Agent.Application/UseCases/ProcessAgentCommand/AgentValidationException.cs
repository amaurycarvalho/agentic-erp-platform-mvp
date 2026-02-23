namespace Agent.Application.UseCases.ProcessAgentCommand;

public sealed class AgentValidationException(string message) : Exception(message);
