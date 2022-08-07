namespace Twilight.CQRS.Interfaces;

/// <summary>
///     Represents base properties for messages. This class cannot be instantiated.
/// </summary>
public interface ICqrsMessage
{
    /// <summary>
    ///     Gets the message identifier.
    /// </summary>
    /// <value>The message identifier.</value>
    string MessageId { get; }

    /// <summary>
    ///     Gets the correlation identifier.
    /// </summary>
    /// <value>The message correlation identifier.</value>
    string CorrelationId { get; }

    /// <summary>
    ///     Gets the session identifier.
    /// </summary>
    /// <value>The session identifier.</value>
    string? SessionId { get; }

    /// <summary>
    ///     Gets the causation identifier.
    /// </summary>
    /// <remarks>Identifies the message (by that message's identifier) that caused a message instance to be produced.</remarks>
    /// <value>The causation identifier.</value>
    string? CausationId { get; }
}
