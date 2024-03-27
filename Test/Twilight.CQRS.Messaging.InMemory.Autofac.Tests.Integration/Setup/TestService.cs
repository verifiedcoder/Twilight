using FluentResults;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;

internal sealed class TestService(IVerifier verifier) : ITestService
{
    public async Task<Result> Receive(string parameters)
        => await verifier.Receive(parameters);
}
