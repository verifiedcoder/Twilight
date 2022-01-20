using Autofac;
using Microsoft.EntityFrameworkCore;
using Twilight.CQRS.Autofac;
using Twilight.CQRS.Messaging.InMemory.Autofac;
using Twilight.Samples.Common;
using Twilight.Samples.Common.Data;

namespace Twilight.Samples.CQRS;

internal sealed class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var cqrsAssembly = typeof(IAssemblyMarker).Assembly;
        var sampleDataContextOptions = new DbContextOptionsBuilder<SampleDataContext>().UseInMemoryDatabase("SampleDataDb").Options;
        var sampleViewContextOptions = new DbContextOptionsBuilder<ViewDataContext>().UseInMemoryDatabase("ViewDataDb").Options;
        var sampleDataContext = new SampleDataContext(sampleDataContextOptions);
        var viewDataContext = new ViewDataContext(sampleViewContextOptions);

        builder.RegisterInstance(sampleDataContext).As<SampleDataContext>();
        builder.RegisterInstance(viewDataContext).As<ViewDataContext>();

        builder.RegisterCqrs(cqrsAssembly)
               .AddAutofacInMemoryMessaging()
               .RegisterAssemblyTypes(ThisAssembly, new[] { "Runner" });
    }
}
