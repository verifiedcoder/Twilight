using Autofac;
using Twilight.CQRS.Autofac;
using Twilight.CQRS.Messaging.InMemory.Autofac;

namespace Twilight.Sample
{
    internal sealed class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = ThisAssembly;

            builder.AddAutofacInMemoryMessaging()
                   .RegisterCqrs(assembly)
                   .RegisterAssemblyTypes(assembly, new[] {"Runner"});
        }
    }
}
