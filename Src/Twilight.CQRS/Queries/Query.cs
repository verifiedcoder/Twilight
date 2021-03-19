using Twilight.CQRS.Contracts;

namespace Twilight.CQRS.Queries
{
    /// <summary>
    ///     <para>
    ///         Represents a result and does not change the observable state of the system (i.e. is free of side-effects).
    ///     </para>
    ///     <para>Implements <see cref="Message" />.</para>
    ///     <para>Implements <see cref="IQuery{TResponse}" />.</para>
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="Message" />
    /// <seealso cref="IQuery{TResponse}" />
    public class Query<TResponse> : Message, IQuery<TResponse>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Query{TResponse}" /> class.
        /// </summary>
        /// <param name="correlationId">The query correlation identifier.</param>
        /// <param name="causationId">
        ///     The causation identifier. Identifies the message that caused this query to be produced.
        ///     Optional.
        /// </param>
        public Query(string correlationId, string? causationId = null)
            : base(correlationId, causationId)
        {
        }
    }

    /// <summary>
    ///     <para>
    ///         Represents a result and does not change the observable state of the system (i.e. is free of side-effects).
    ///     </para>
    ///     <para>Implements <see cref="Message" />.</para>
    ///     <para>Implements <see cref="IQuery{TResponse}" />.</para>
    /// </summary>
    /// <typeparam name="TParameters">The type of the parameters.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="Message" />
    /// <seealso cref="IQuery{TResponse}" />
    public class Query<TParameters, TResponse> : Message, IQuery<TResponse>
        where TParameters : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Query{TParameters, TResponse}" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="correlationId">The query correlation identifier.</param>
        /// <param name="causationId">
        ///     The causation identifier. Identifies the message that caused this query to be produced.
        ///     Optional.
        /// </param>
        public Query(TParameters parameters, string correlationId, string? causationId = null)
            : base(correlationId, causationId) => Params = parameters;

        /// <summary>
        ///     Gets the typed query parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public TParameters Params { get; }
    }
}
