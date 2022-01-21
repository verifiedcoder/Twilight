using FluentValidation;
using Twilight.CQRS.Events;

namespace Twilight.CQRS.Benchmarks.Queries;

internal sealed class SendQueryValidator : AbstractValidator<CqrsEvent<MessageParameters>>
{
    public SendQueryValidator()
    {
        RuleFor(p => p.Params).NotNull();
        RuleFor(p => p.CorrelationId).NotEmpty();
        RuleFor(p => p.Params.Message).NotEmpty();
    }
}
