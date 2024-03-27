using Twilight.CQRS.Events;

namespace Twilight.CQRS.Benchmarks.Events;

internal sealed class SendCqrsEvent(MessageParameters parameters, string correlationId, string? causationId = null) : CqrsEvent<MessageParameters>(parameters, correlationId, causationId);
