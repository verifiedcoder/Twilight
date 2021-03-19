using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
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
        protected QueryHandlerBase(IValidator<TQuery>? validator = default)
            : base(validator)
        {
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default)
        {
            var activitySource = new ActivitySource(ActivitySourceName, AssemblyVersion);

            using var activity = activitySource.StartActivity($"Handle {query.GetType()}");
            {
                using (var childSpan = activitySource.StartActivity("OnBeforeHandlingQuery", ActivityKind.Consumer))
                {
                    childSpan?.AddEvent(new ActivityEvent($"{nameof(QueryHandlerBase<TQuery, TResponse>)}.{nameof(OnBeforeHandling)}"));

                    await OnBeforeHandling(query, cancellationToken);
                }

                using (var childSpan = activitySource.StartActivity("ValidateQuery", ActivityKind.Consumer))
                {
                    childSpan?.AddEvent(new ActivityEvent($"{nameof(QueryHandlerBase<TQuery, TResponse>)}.{nameof(ValidateMessage)}"));

                    await ValidateMessage(query, cancellationToken);
                }

                TResponse response;

                using (var childSpan = activitySource.StartActivity("HandleQuery", ActivityKind.Consumer))
                {
                    childSpan?.AddEvent(new ActivityEvent($"{nameof(QueryHandlerBase<TQuery, TResponse>)}.{nameof(HandleQuery)}"));

                    response = await HandleQuery(query, cancellationToken);
                }

                using (var childSpan = activitySource.StartActivity("OnAfterHandlingQuery", ActivityKind.Consumer))
                {
                    childSpan?.AddEvent(new ActivityEvent($"{nameof(QueryHandlerBase<TQuery, TResponse>)}.{nameof(OnAfterHandling)}"));

                    await OnAfterHandling(query, cancellationToken);
                }

                return response;
            }
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
