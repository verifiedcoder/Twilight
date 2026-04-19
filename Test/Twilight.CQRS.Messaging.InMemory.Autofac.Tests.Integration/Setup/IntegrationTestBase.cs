using Autofac;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    private readonly IContainer _container;

    protected IntegrationTestBase()
    {
        Verifier = Substitute.For<IVerifier>();

        Verifier.Receive(Arg.Any<string>()).Returns(Result.Ok());

        var builder = new ContainerBuilder();
        
        // Explicit variable type is required for service resolution.
        ITestService testService = new TestService(Verifier);

        builder.RegisterModule<IntegrationTestModule>();
        builder.RegisterInstance(testService);

        _container = builder.Build();

        Subject = _container.Resolve<IMessageSender>();
    }

    protected IMessageSender Subject { get; }

    protected IVerifier Verifier { get; }

    /// <inheritdoc />
    public async ValueTask InitializeAsync()
        => await Task.CompletedTask;

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        _container.Dispose();

        await Task.CompletedTask;

        GC.SuppressFinalize(this);
    }
}
