using Autofac;
using Microsoft.EntityFrameworkCore;
using Twilight.CQRS.Autofac;
using Twilight.CQRS.Messaging.InMemory.Autofac;
using Twilight.Samples.CQRS.Data;

namespace Twilight.Samples.CQRS;

internal sealed class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = ThisAssembly;
        var sampleDataContextOptions = new DbContextOptionsBuilder<SampleDataContext>().UseInMemoryDatabase("InMemory").Options;
        var sampleViewContextOptions = new DbContextOptionsBuilder<ViewDataContext>().UseInMemoryDatabase("InMemory").Options;

        SampleDataContext sampleDataContext = new (sampleDataContextOptions);
        ViewDataContext viewDataContext = new (sampleViewContextOptions);

        builder.RegisterInstance(sampleDataContext).As<SampleDataContext>();
        builder.RegisterInstance(viewDataContext).As<ViewDataContext>();

        builder.RegisterCqrs(assembly)
                .AddAutofacInMemoryMessaging()
                .RegisterAssemblyTypes(assembly, new[] {"Runner"});
    }
}
