namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;

public sealed class TestService : ITestService
{
    private readonly IVerifier _verifier;

    public TestService(IVerifier verifier)
        => _verifier = verifier;

    public async Task Receive(string parameters)
        => await _verifier.Receive(parameters);
}
