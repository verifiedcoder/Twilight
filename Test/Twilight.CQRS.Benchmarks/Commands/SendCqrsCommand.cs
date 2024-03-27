using Twilight.CQRS.Commands;

namespace Twilight.CQRS.Benchmarks.Commands;

internal sealed class SendCqrsCommand(MessageParameters parameters, string correlationId, string? causationId = null) : CqrsCommand<MessageParameters>(parameters, correlationId, causationId);
