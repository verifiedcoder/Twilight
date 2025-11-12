using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS;

/// <inheritdoc />
/// <param name="correlationId">The message correlation identifier.</param>
/// <param name="sessionId">The session identifier for the session to which this message belongs.</param>
/// <param name="causationId">
///     The causation identifier. Identifies the message that caused this message to be produced.
///     Optional.
/// </param>
public abstract class CqrsMessage(string correlationId, string? sessionId = null, string? causationId = null) : ICqrsMessage
{
    /// <inheritdoc />
    public string MessageId { get; } = Guid.NewGuid().ToString();

    /// <inheritdoc />
    public string CorrelationId { get; } = correlationId;

    /// <inheritdoc />
    public string? SessionId { get; } = sessionId;

    /// <inheritdoc />
    public string? CausationId { get; } = causationId;
}
