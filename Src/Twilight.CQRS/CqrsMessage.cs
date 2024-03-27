using Taikandi;
using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS;

/// <inheritdoc />
public abstract class CqrsMessage : ICqrsMessage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsMessage" /> class.
    /// </summary>
    /// <param name="correlationId">The message correlation identifier.</param>
    /// <param name="sessionId">The session identifier for the session to which this message belongs.</param>
    /// <param name="causationId">
    ///     The causation identifier. Identifies the message that caused this message to be produced.
    ///     Optional.
    /// </param>
    protected CqrsMessage(string correlationId, string? sessionId = null, string? causationId = null)
    {
        MessageId = SequentialGuid.NewGuid().ToString();
        CausationId = causationId;
        SessionId = sessionId;
        CorrelationId = correlationId;
    }

    /// <inheritdoc />
    public string MessageId { get; }

    /// <inheritdoc />
    public string CorrelationId { get; }

    /// <inheritdoc />
    public string? SessionId { get; }

    /// <inheritdoc />
    public string? CausationId { get; }
}
