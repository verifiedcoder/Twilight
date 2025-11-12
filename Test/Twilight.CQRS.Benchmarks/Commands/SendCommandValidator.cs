using Twilight.CQRS.Commands;

namespace Twilight.CQRS.Benchmarks.Commands;

[UsedImplicitly]
internal sealed class SendCommandValidator : AbstractValidator<CqrsCommand<MessageParameters>>
{
    public SendCommandValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.CorrelationId).NotEmpty();
        RuleFor(p => p.Params.Message).NotEmpty();
    }
}
