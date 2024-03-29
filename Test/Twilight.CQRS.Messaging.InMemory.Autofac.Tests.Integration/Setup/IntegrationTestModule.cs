﻿using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Twilight.CQRS.Autofac;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;

internal sealed class IntegrationTestModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterCqrs([ThisAssembly]);
        builder.AddAutofacInMemoryMessaging();
        builder.RegisterGeneric(typeof(NullLogger<>)).As(typeof(ILogger<>));
    }
}
