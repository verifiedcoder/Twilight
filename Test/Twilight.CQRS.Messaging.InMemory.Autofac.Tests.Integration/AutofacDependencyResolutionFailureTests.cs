using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration;

public sealed class AutofacDependencyResolutionFailureTests : IAsyncLifetime
{
    private static IContainer? _container;

    private readonly IMessageSender _subject;

    public AutofacDependencyResolutionFailureTests()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<AutofacInMemoryMessageSender>().As<IMessageSender>();
        builder.RegisterType<NullLogger<AutofacInMemoryMessageSender>>().As<ILogger<AutofacInMemoryMessageSender>>();

        _container = builder.Build();

        _subject = _container.Resolve<IMessageSender>();
    }

    public async Task InitializeAsync()
        => await Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _container?.Dispose();

        await Task.CompletedTask;
    }

    [Fact]
    public async Task MessageSender_ThrowsWhenDependencyResolutionFails()
    {
        // Arrange
        var parameters = new MultipleHandlersParameters();
        var command = new CqrsCommand<MultipleHandlersParameters>(parameters, Constants.CorrelationId);

        // Act
        var result = await _subject.Send(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBe(false);
    }
}
