using Twilight.CQRS.Events;

namespace Twilight.CQRS.Benchmarks.Events;

[UsedImplicitly]
internal sealed class SendEventValidator : AbstractValidator<CqrsEvent<MessageParameters>>
{
    public SendEventValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.CorrelationId).NotEmpty();
        RuleFor(p => p.Params.Message).NotEmpty();
    }
}
