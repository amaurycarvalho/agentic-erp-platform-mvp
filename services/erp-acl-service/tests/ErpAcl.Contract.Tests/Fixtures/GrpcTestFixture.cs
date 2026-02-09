using Microsoft.AspNetCore.Mvc.Testing;
using Grpc.Net.Client;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ErpAcl.Contract.Tests.Fixtures;

public class GrpcTestFixture : IAsyncLifetime
{
    public GrpcChannel Channel { get; private set; } = null!;
    private WebApplicationFactory<Program> _factory = null!;

    public Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>();
        Channel = GrpcChannel.ForAddress(
            _factory.Server.BaseAddress!,
            new GrpcChannelOptions
            {
                HttpHandler = _factory.Server.CreateHandler()
            });

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        Channel.Dispose();
        _factory.Dispose();
        return Task.CompletedTask;
    }
}
