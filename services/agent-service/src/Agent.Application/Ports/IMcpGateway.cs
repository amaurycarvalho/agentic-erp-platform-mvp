using System.Text.Json;

namespace Agent.Application.Ports;

public interface IMcpGateway
{
    Task<JsonElement> ExecuteToolAsync(string toolName, object payload, CancellationToken cancellationToken);
}
