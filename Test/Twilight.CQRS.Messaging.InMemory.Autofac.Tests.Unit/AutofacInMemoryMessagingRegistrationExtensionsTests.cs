using Autofac;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Twilight.CQRS.Messaging.Interfaces;
using Xunit;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Unit;

public sealed class AutofacInMemoryMessagingRegistrationExtensionsTests
{
    [Fact]
    public void AddInMemoryMessagingShouldRegisterInMemoryMessageSender()
    {
        // Arrange
        var builder = new ContainerBuilder();

        builder.AddAutofacInMemoryMessaging();
        builder.RegisterType<NullLogger<AutofacInMemoryMessageSender>>().As<ILogger<AutofacInMemoryMessageSender>>();

        // Act
        var container = builder.Build();

        // Assert
        container.ComponentRegistry.Registrations.Count().Should().Be(3);

        container.ComponentRegistry
                    .Registrations.Any(x => x.Services.Any(s => s.Description == typeof(IMessageSender).FullName))
                    .Should()
                    .BeTrue();

        var messageSender = container.Resolve<IMessageSender>();

        messageSender.Should().BeAssignableTo<AutofacInMemoryMessageSender>();

        container.Dispose();
    }
}
