using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Twilight.CQRS.Autofac;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;

public sealed class IntegrationTestModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterCqrs(new[] { ThisAssembly });
        builder.AddAutofacInMemoryMessaging();
        builder.RegisterGeneric(typeof(NullLogger<>)).As(typeof(ILogger<>));
    }
}
