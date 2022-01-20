using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Autofac.Tests.Unit.Setup;

internal sealed class TestCqrsCommandHandler : CqrsCommandHandlerBase<TestCqrsCommandHandler, CqrsCommand<TestParameters>>
{
    public TestCqrsCommandHandler(IMessageSender messageSender,
                                  ILogger<TestCqrsCommandHandler> logger,
                                  IValidator<CqrsCommand<TestParameters>> validator)
        : base(messageSender, logger, validator)
    {
    }

    public override async Task HandleCommand(CqrsCommand<TestParameters> cqrsCommand, CancellationToken cancellationToken = default)
        => await Task.CompletedTask;
}
