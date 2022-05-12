using Autofac;
using Twilight.CQRS.Autofac;
using Twilight.CQRS.Messaging.InMemory.Autofac;
using Twilight.Samples.Common;

namespace Twilight.Samples.CQRS;

internal sealed class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
        => builder.RegisterCqrs(typeof(IAssemblyMarker).Assembly)
                  .AddAutofacInMemoryMessaging()
                  .RegisterAssemblyTypes(ThisAssembly, new[] { "Runner" });
}
