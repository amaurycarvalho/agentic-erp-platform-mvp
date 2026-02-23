namespace Agent.Application.UseCases.ProcessAgentCommand;

public sealed class UnsupportedIntentException(string message) : Exception(message);
