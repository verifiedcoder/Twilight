using System.Diagnostics;
using FluentValidation;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Diagnostics;
using Twilight.CQRS.Interfaces;
// ReSharper disable ExplicitCallerInfoArgument as false positive for StartActivity

namespace Twilight.CQRS.Queries;

/// <summary>
///     <para>
///         Represents the ability to process (handle) queries. A query handler receives a query and directs the query
///         payload for processing. This class cannot be instantiated.
///     </para>
///     <para>Implements <see cref="CqrsMessageHandler{THandler,TMessage}" />.</para>
///     <para>Implements <see cref="ICqrsQueryHandler{TQuery,TResponse}" />.</para>
/// </summary>
/// <typeparam name="TQuery">The type of the query.</typeparam>
/// <typeparam name="TResponse">The type of the query response.</typeparam>
/// <typeparam name="TQueryHandler">The type of the query handler.</typeparam>
/// <seealso cref="CqrsMessageHandler{THandler,TMessage}" />
/// <seealso cref="ICqrsQueryHandler{TQuery,TResponse}" />
public abstract class CqrsQueryHandlerBase<TQueryHandler, TQuery, TResponse> : CqrsMessageHandler<TQueryHandler, TQuery>, ICqrsQueryHandler<TQuery, TResponse>
    where TQuery : class, ICqrsQuery<TResponse>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsQueryHandlerBase{TQueryHandler,TQuery,TResponse}" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="validator">The query validator.</param>
    protected CqrsQueryHandlerBase(ILogger<TQueryHandler> logger, IValidator<TQuery>? validator = default)
        : base(logger, validator)
    {
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(query, nameof(query));

        using var activity = Activity.Current?.Source.StartActivity($"Handle {query.GetType()}");
        {
            using (var childSpan = Activity.Current?.Source.StartActivity("Pre query handling logic"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsQueryHandlerBase<TQueryHandler, TQuery, TResponse>)}.{nameof(OnBeforeHandling)}"));

                await OnBeforeHandling(query, cancellationToken);
            }

            using (var childSpan = Activity.Current?.Source.StartActivity("Validate query"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsQueryHandlerBase<TQueryHandler, TQuery, TResponse>)}.{nameof(ValidateMessage)}"));

                await ValidateMessage(query, cancellationToken);
            }

            TResponse response;

            using (var childSpan = Activity.Current?.Source.StartActivity("Handle query"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsQueryHandlerBase<TQueryHandler, TQuery, TResponse>)}.{nameof(HandleQuery)}"));

                response = await HandleQuery(query, cancellationToken);
            }

            using (var childSpan = Activity.Current?.Source.StartActivity("Post query handling logic"))
            {
                childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsQueryHandlerBase<TQueryHandler, TQuery, TResponse>)}.{nameof(OnAfterHandling)}"));

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
