using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Twilight.CQRS.Autofac;
using Twilight.CQRS.Messaging.InMemory.Autofac;

namespace Twilight.CQRS.Benchmarks;

internal class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterCqrs(ThisAssembly);
        builder.AddAutofacInMemoryMessaging();

        builder.RegisterGeneric(typeof(NullLogger<>)).As(typeof(ILogger<>)).SingleInstance();
    }
}
