using Microsoft.Toolkit.Diagnostics;
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
    /// <param name="causationId">
    ///     The causation identifier. Identifies the message that caused this message to be produced.
    ///     Optional.
    /// </param>
    protected CqrsMessage(string correlationId, string? causationId = null)
    {
        Guard.IsNotEmpty(correlationId, nameof(correlationId));

        MessageId = SequentialGuid.NewGuid().ToString();
        CorrelationId = correlationId;
        CausationId = causationId;
    }

    /// <inheritdoc />
    public string MessageId { get; }

    /// <inheritdoc />
    public string CorrelationId { get; }

    /// <inheritdoc />
    public string? CausationId { get; }
}
