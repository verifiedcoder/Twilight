using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
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
/// <param name="logger">The logger.</param>
/// <param name="validator">The query validator.</param>
/// <seealso cref="CqrsMessageHandler{THandler,TMessage}" />
/// <seealso cref="ICqrsQueryHandler{TQuery,TResponse}" />
public abstract class CqrsQueryHandlerBase<TQueryHandler, TQuery, TResponse>(
    ILogger<CqrsQueryHandlerBase<TQueryHandler, TQuery, TResponse>> logger, 
    IValidator<TQuery>? validator = null) : CqrsMessageHandler<TQueryHandler, TQuery>(logger, validator), ICqrsQueryHandler<TQuery, TResponse>
    where TQuery : class, ICqrsQuery<TResponse>
{
    /// <inheritdoc />
    public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        var guardResult = Result.Try(() => Guard.IsNotNull(query));

        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        using var activity = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity($"Handle {query.GetType()}") : null;

        var preHandlingResult = await ExecutePreHandlingAsync(query, cancellationToken);

        if (!preHandlingResult.IsSuccess)
        {
            return preHandlingResult;
        }

        var validationResult = await ExecuteValidationAsync(query, cancellationToken);

        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var queryResult = await ExecuteHandleQueryAsync(query, cancellationToken);

        if (!queryResult.IsSuccess)
        {
            return queryResult;
        }

        var postHandlingResult = await ExecutePostHandlingAsync(query, cancellationToken);

        return postHandlingResult.IsSuccess
            ? queryResult
            : postHandlingResult;
    }

    private static bool ShouldCreateActivity() => Activity.Current?.Source.HasListeners() ?? false;

    /// <summary>
    ///     Handles the query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     <para>A task that represents the asynchronous query handler operation.</para>
    ///     <para>The task result contains the query execution response.</para>
    /// </returns>
    protected abstract Task<Result<TResponse>> HandleQuery(TQuery query, CancellationToken cancellationToken = default);

    private async Task<Result<TResponse>> ExecutePreHandlingAsync(TQuery query, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Pre query handling logic") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsQueryHandlerBase<,,>)}.{nameof(OnBeforeHandling)}"));

        return await OnBeforeHandling(query, cancellationToken);
    }

    private async Task<Result<TResponse>> ExecuteValidationAsync(TQuery query, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Validate query") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsQueryHandlerBase<,,>)}.{nameof(ValidateMessage)}"));

        return await ValidateMessage(query, cancellationToken);
    }

    private async Task<Result<TResponse>> ExecuteHandleQueryAsync(TQuery query, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Handle query") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsQueryHandlerBase<,,>)}.{nameof(HandleQuery)}"));

        return await HandleQuery(query, cancellationToken);
    }

    private async Task<Result<TResponse>> ExecutePostHandlingAsync(TQuery query, CancellationToken cancellationToken)
    {
        using var childSpan = ShouldCreateActivity() ? Activity.Current?.Source.StartActivity("Post query handling logic") : null;

        childSpan?.AddEvent(new ActivityEvent($"{nameof(CqrsQueryHandlerBase<,,>)}.{nameof(OnAfterHandling)}"));

        return await OnAfterHandling(query, cancellationToken);
    }
}
