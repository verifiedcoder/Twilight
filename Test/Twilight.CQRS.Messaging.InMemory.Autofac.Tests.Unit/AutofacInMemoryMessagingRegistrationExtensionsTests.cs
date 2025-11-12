using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Unit;

public sealed class AutofacInMemoryMessagingRegistrationExtensionsTests
{
    [Fact]
    public void AddInMemoryMessaging_ShouldRegister_InMemoryMessageSender()
    {
        // Arrange
        var builder = new ContainerBuilder();

        builder.AddAutofacInMemoryMessaging();
        builder.RegisterType<NullLogger<AutofacInMemoryMessageSender>>().As<ILogger<AutofacInMemoryMessageSender>>();

        // Act
        var container = builder.Build();

        // Assert
        container.ComponentRegistry.Registrations.Count().ShouldBe(3);

        container.ComponentRegistry
                 .Registrations.Any(x => x.Services.Any(s => s.Description == typeof(IMessageSender).FullName))
                 .ShouldBeTrue();

        var messageSender = container.Resolve<IMessageSender>();

        messageSender.ShouldBeAssignableTo<AutofacInMemoryMessageSender>();

        container.Dispose();
    }
}
