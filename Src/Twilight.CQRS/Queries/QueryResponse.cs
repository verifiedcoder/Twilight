namespace Twilight.CQRS.Queries
{
    /// <summary>
    ///     <para>Represents an encapsulated response from a query handler.</para>
    ///     <para>Implements <see cref="Message" />.</para>
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload.</typeparam>
    /// <seealso cref="Message" />
    public class QueryResponse<TPayload> : Message
        where TPayload : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QueryResponse{TPayload}" /> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="correlationId">The message correlation identifier.</param>
        /// <param name="causationId">
        ///     The causation identifier. Identifies the query that caused this response to be produced.
        ///     Optional.
        /// </param>
        public QueryResponse(TPayload payload, string correlationId, string? causationId = null)
            : base(correlationId, causationId) => Payload = payload;

        /// <summary>
        ///     Gets the typed query response payload.
        /// </summary>
        /// <value>The payload.</value>
        public TPayload Payload { get; }
    }
}
