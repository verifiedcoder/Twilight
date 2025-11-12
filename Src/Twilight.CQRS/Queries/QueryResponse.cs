namespace Twilight.CQRS.Queries;

/// <summary>
///     <para>Represents an encapsulated response from a query handler.</para>
///     <para>Implements <see cref="CqrsMessage" />.</para>
/// </summary>
/// <typeparam name="TPayload">The type of the payload.</typeparam>
/// <param name="payload">The payload.</param>
/// <param name="correlationId">The message correlation identifier.</param>
/// <param name="sessionId">The session identifier.</param>
/// <param name="causationId">
///     The causation identifier. Identifies the query that caused this response to be produced.
///     Optional.
/// </param>
/// <seealso cref="CqrsMessage" />
public class QueryResponse<TPayload>(
    TPayload payload, 
    string correlationId,
    string? sessionId = null,
    string? causationId = null) : CqrsMessage(correlationId, sessionId, causationId)
    where TPayload : class
{
    /// <summary>
    ///     Gets the typed query response payload.
    /// </summary>
    /// <value>The payload.</value>
    public TPayload Payload { get; } = payload;
}
