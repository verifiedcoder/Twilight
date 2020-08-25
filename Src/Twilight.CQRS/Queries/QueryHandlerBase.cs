using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Contracts;

namespace Twilight.CQRS.Queries
{
    /// <summary>
    ///     <para>
    ///         Represents the ability to process (handle) queries. A query handler receives a query and directs the query for
    ///         processing. This class cannot be instantiated.
    ///     </para>
    ///     <para>Implements <see cref="MessageHandler{TQuery}" />.</para>
    ///     <para>Implements <see cref="IQueryHandler{TQuery,TResponse}" />.</para>
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResponse">The type of the query response.</typeparam>
    /// <seealso cref="MessageHandler{TQuery}" />
    /// <seealso cref="IQueryHandler{TQuery, TResponse}" />
    public abstract class QueryHandlerBase<TQuery, TResponse> : MessageHandler<TQuery>, IQueryHandler<TQuery, TResponse>
        where TQuery : class, IQuery<TResponse>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QueryHandlerBase{TQuery,TResponse}" /> class.
        /// </summary>
        /// <param name="validator">The query validator.</param>
        /// <param name="logger">The logger.</param>
        protected QueryHandlerBase(ILogger<IMessageHandler<TQuery>> logger, IValidator<TQuery>? validator = default)
            : base(logger, validator)
        {
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default)
        {
            await OnBeforeHandling(query, cancellationToken);

            await ValidateMessage(query, cancellationToken);

            var response = await HandleQuery(query, cancellationToken);

            await OnAfterHandling(query, cancellationToken);

            return response;
        }

        /// <summary>
        ///     Handles the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     <para>A task that represents the asynchronous query handler operation.</para>
        ///     <para>The task result contains the query execution response.</para>
        /// </returns>
        protected abstract Task<TResponse> HandleQuery(TQuery query, CancellationToken cancellationToken = default);
    }
}
