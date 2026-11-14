namespace Twilight.CQRS.Benchmarks.Commands;

[UsedImplicitly]
internal sealed class SendCommandValidator : AbstractValidator<SendCqrsCommand>
{
    public SendCommandValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.CorrelationId).NotEmpty();
        RuleFor(p => p.Params.Message).NotEmpty();
    }
}
