﻿using Autofac;
using FluentAssertions;
using FluentResults;
using NSubstitute;
using Twilight.CQRS.Messaging.Interfaces;
using Xunit;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    private readonly IContainer _container;

    protected IntegrationTestBase()
    {
        Verifier = Substitute.For<IVerifier>();

        Verifier.Receive(Arg.Any<string>()).Returns(Result.Ok());

        var builder = new ContainerBuilder();
        var testService = new TestService(Verifier).As<ITestService>();

        builder.RegisterModule<IntegrationTestModule>();
        builder.RegisterInstance(testService);

        _container = builder.Build();

        Subject = _container.Resolve<IMessageSender>();
    }

    protected IMessageSender Subject { get; }

    protected IVerifier Verifier { get; }

    public async Task InitializeAsync()
        => await Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _container.Dispose();

        await Task.CompletedTask;
    }
}
