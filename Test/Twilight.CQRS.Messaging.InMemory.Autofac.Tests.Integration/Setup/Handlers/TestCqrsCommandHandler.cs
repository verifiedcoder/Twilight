using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Commands;
using Twilight.CQRS.Messaging.Interfaces;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Handlers;

internal sealed class TestCqrsCommandHandler : CqrsCommandHandlerBase<TestCqrsCommandHandler, CqrsCommand<TestParameters>>
{
    private readonly ITestService _service;

    public TestCqrsCommandHandler(ITestService service,
                                  IMessageSender messageSender,
                                  ILogger<TestCqrsCommandHandler> logger,
                                  IValidator<CqrsCommand<TestParameters>> validator)
        : base(messageSender, logger, validator)
        => _service = service;

    public override async Task HandleCommand(CqrsCommand<TestParameters> command, CancellationToken cancellationToken = default)
        => await _service.Receive(command.Params.Value);
}
