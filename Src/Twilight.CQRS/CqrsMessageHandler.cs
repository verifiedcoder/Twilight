using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS;

/// <inheritdoc />
/// <param name="logger">The logger.</param>
/// <param name="validator">The message validator.</param>
public abstract class CqrsMessageHandler<THandler, TMessage>(
    ILogger<CqrsMessageHandler<THandler, TMessage>> logger, 
    IValidator<TMessage>? validator = null) : ICqrsMessageHandler<TMessage>
    where TMessage : class
{
    /// <summary>
    ///     Gets the message handler logger.
    /// </summary>
    /// <value>The logger.</value>
    protected ILogger<CqrsMessageHandler<THandler, TMessage>> Logger { get; } = logger;

    /// <inheritdoc />
    public virtual Task<Result> OnBeforeHandling(TMessage message, CancellationToken cancellationToken = default)
        => Task.FromResult(Result.Try(() =>
        {
            Guard.IsNotNull(message);
        }));

    /// <inheritdoc />
    public virtual async Task<Result> ValidateMessage(TMessage message, CancellationToken cancellationToken = default)
    {
        if (validator is null)
        {
            return Result.Ok();
        }

        var guardResult = Result.Try(() =>
        {
            Guard.IsNotNull(message);
        });

        if (guardResult.IsFailed)
        {
            return guardResult;
        }

        try
        {
            await validator.ValidateAndThrowAsync(message, cancellationToken);

            return Result.Ok();
        }
        catch (ValidationException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    /// <inheritdoc />
    public virtual Task<Result> OnAfterHandling(TMessage message, CancellationToken cancellationToken = default)
        => Task.FromResult(Result.Try(() =>
        {
            Guard.IsNotNull(message);
        }));
}
