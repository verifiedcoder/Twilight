﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Contracts;
using Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Parameters;
using Twilight.CQRS.Messaging.Shared;
using Twilight.CQRS.Tests.Unit.Shared;
using Xunit;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration
{
    public sealed class AutofacDependencyResolutionExceptionTests : IAsyncLifetime
    {
        private static IContainer? _container;

        private readonly IMessageSender _subject;

        public AutofacDependencyResolutionExceptionTests()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<AutofacInMemoryMessageSender>().As<IMessageSender>();
            builder.RegisterType<NullLogger<AutofacInMemoryMessageSender>>().As<ILogger<AutofacInMemoryMessageSender>>();

            _container = builder.Build();

            _subject = _container.Resolve<IMessageSender>();
        }

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            _container?.Dispose();

            await Task.CompletedTask;
        }

        [Fact]
        public async Task MessageSenderThrowsWhenDependencyResolutionFails()
        {
            var parameters = new MultipleHandlersParameters();
            var command = new Command<MultipleHandlersParameters>(parameters, Constants.CorrelationId);

            Func<Task> subjectResult = async () => { await _subject.Send(command, CancellationToken.None); };

            await subjectResult.Should().ThrowAsync<HandlerNotFoundException>();
        }
    }
}
