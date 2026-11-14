namespace Twilight.CQRS.Benchmarks.Queries;

[UsedImplicitly]
internal sealed class SendQueryValidator : AbstractValidator<SendCqrsQuery>
{
    public SendQueryValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.CorrelationId).NotEmpty();
        RuleFor(p => p.Params.Message).NotEmpty();
    }
}
