using Twilight.CQRS.Commands;
using Twilight.CQRS.Events;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Common;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;

[UsedImplicitly]
internal sealed class MultipleHandlersValidator : AbstractValidator<CqrsCommand<MultipleHandlersParameters>>;

[UsedImplicitly]
internal sealed class TestCommandValidator : AbstractValidator<CqrsCommand<TestParameters>>;

[UsedImplicitly]
internal sealed class TestCommandWithResponseValidator : AbstractValidator<CqrsCommand<TestParameters, CqrsCommandResponse<string>>>;

[UsedImplicitly]
internal sealed class TestEventValidator : AbstractValidator<CqrsEvent<TestParameters>>;

[UsedImplicitly]
internal sealed class TestQueryValidator : AbstractValidator<CqrsQuery<TestParameters, QueryResponse<string>>>;
