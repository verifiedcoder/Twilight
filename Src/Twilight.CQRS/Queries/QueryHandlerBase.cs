using System.Diagnostics;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Diagnostics;
using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS.Queries;

/// <summary>
///     <para>
///         Represents the ability to process (handle) queries. A query handler receives a query and directs the query for
///         processing. This class cannot be instantiated.
///     </para>
///     <para>Implements <see cref="MessageHandler{TQueryHandler, TQuery}" />.</para>
///     <para>Implements <see cref="IQueryHandler{TQuery,TResponse}" />.</para>
/// </summary>
/// <typeparam name="TQuery">The type of the query.</typeparam>
/// <typeparam name="TResponse">The type of the query response.</typeparam>
/// <typeparam name="TQueryHandler">The type of the query handler.</typeparam>
/// <seealso cref="MessageHandler{TQueryHandler, TQuery}" />
/// <seealso cref="IQueryHandler{TQuery, TResponse}" />
public abstract class QueryHandlerBase<TQueryHandler, TQuery, TResponse> : MessageHandler<TQueryHandler, TQuery>, IQueryHandler<TQuery, TResponse>
    where TQuery : class, IQuery<TResponse>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="QueryHandlerBase{TQueryHandler, TQuery,TResponse}" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="validator">The query validator.</param>
    protected QueryHandlerBase(ILogger<TQueryHandler> logger, IValidator<TQuery>? validator = default)
        : base(logger, validator)
    {
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(query, nameof(query));

        var activitySource = new ActivitySource(ActivitySourceName, AssemblyVersion);

        using var activity = activitySource.StartActivity($"Handle {query.GetType()}");
        {
            using (var childSpan = activitySource.StartActivity("OnBeforeHandlingQuery", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(QueryHandlerBase<TQueryHandler, TQuery, TResponse>)}.{nameof(OnBeforeHandling)}"));

                await OnBeforeHandling(query, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity("ValidateQuery", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(QueryHandlerBase<TQueryHandler, TQuery, TResponse>)}.{nameof(ValidateMessage)}"));

                await ValidateMessage(query, cancellationToken);
            }

            TResponse response;

            using (var childSpan = activitySource.StartActivity("HandleQuery", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(QueryHandlerBase<TQueryHandler, TQuery, TResponse>)}.{nameof(HandleQuery)}"));

                response = await HandleQuery(query, cancellationToken);
            }

            using (var childSpan = activitySource.StartActivity("OnAfterHandlingQuery", ActivityKind.Consumer))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(QueryHandlerBase<TQueryHandler, TQuery, TResponse>)}.{nameof(OnAfterHandling)}"));

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

