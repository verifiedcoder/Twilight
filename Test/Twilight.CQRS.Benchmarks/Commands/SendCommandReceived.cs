using Twilight.CQRS.Events;

namespace Twilight.CQRS.Benchmarks.Commands;

internal sealed class SendCommandReceived(string correlationId, string? causationId = null) : CqrsEvent(correlationId, causationId);
