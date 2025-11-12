using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS.Queries;

/// <summary>
///     <para>
///         Represents a result and does not change the observable state of the system (i.e. is free of side effects).
///     </para>
///     <para>Implements <see cref="CqrsMessage" />.</para>
///     <para>Implements <see cref="ICqrsQuery{TResponse}" />.</para>
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <param name="correlationId">The query correlation identifier.</param>
/// <param name="sessionId">The session identifier.</param>
/// <param name="causationId">
///     The causation identifier. Identifies the message that caused this query to be produced.
///     Optional.
/// </param>
/// <seealso cref="CqrsMessage" />
/// <seealso cref="ICqrsQuery{TResponse}" />
public class CqrsQuery<TResponse>(
    string correlationId,
    string? sessionId = null,
    string? causationId = null) : CqrsMessage(correlationId, sessionId, causationId), ICqrsQuery<TResponse>
    where TResponse : class;

/// <summary>
///     <para>
///         Represents a result and does not change the observable state of the system (i.e. is free of side effects).
///     </para>
///     <para>Implements <see cref="CqrsMessage" />.</para>
///     <para>Implements <see cref="ICqrsQuery{TResponse}" />.</para>
/// </summary>
/// <typeparam name="TParameters">The type of the parameters.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <param name="parameters">The parameters.</param>
/// <param name="correlationId">The query correlation identifier.</param>
/// <param name="sessionId">The session identifier.</param>
/// <param name="causationId">
///     The causation identifier. Identifies the message that caused this query to be produced.
///     Optional.
/// </param>
/// <seealso cref="CqrsMessage" />
/// <seealso cref="ICqrsQuery{TResponse}" />
public class CqrsQuery<TParameters, TResponse>(
    TParameters parameters,
    string correlationId,
    string? sessionId = null, 
    string? causationId = null) : CqrsMessage(correlationId, sessionId, causationId), ICqrsQuery<TResponse>
    where TParameters : class
    where TResponse : class
{
    /// <summary>
    ///     Gets the typed query parameters.
    /// </summary>
    /// <value>The parameters.</value>
    public TParameters Params { get; } = parameters;
}
