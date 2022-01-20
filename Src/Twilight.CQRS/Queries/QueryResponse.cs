﻿using Microsoft.Toolkit.Diagnostics;

namespace Twilight.CQRS.Queries;

/// <summary>
///     <para>Represents an encapsulated response from a query handler.</para>
///     <para>Implements <see cref="CqrsMessage" />.</para>
/// </summary>
/// <typeparam name="TPayload">The type of the payload.</typeparam>
/// <seealso cref="CqrsMessage" />
public class QueryResponse<TPayload> : CqrsMessage
    where TPayload : class
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="QueryResponse{TPayload}" /> class.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="correlationId">The message correlation identifier.</param>
    /// <param name="causationId">
    ///     The causation identifier. Identifies the cqrsQuery that caused this response to be produced.
    ///     Optional.
    /// </param>
    public QueryResponse(TPayload payload, string correlationId, string? causationId = null)
        : base(correlationId, causationId)
    {
        Guard.IsNotNull(payload, nameof(payload));

        Payload = payload;
    }

    /// <summary>
    ///     Gets the typed query response payload.
    /// </summary>
    /// <value>The payload.</value>
    public TPayload Payload { get; }
}
