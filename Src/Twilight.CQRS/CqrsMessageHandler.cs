using FluentValidation;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Diagnostics;
using FluentResults;
using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS;

/// <inheritdoc />
public abstract class CqrsMessageHandler<THandler, TMessage> : ICqrsMessageHandler<TMessage>
    where TMessage : class
{
    private readonly IValidator<TMessage>? _validator;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsMessageHandler{THandler,TMessage}" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="validator">The message validator.</param>
    protected CqrsMessageHandler(ILogger<THandler> logger, IValidator<TMessage>? validator = default)
    {
        _validator = validator;

        Logger = logger;
    }

    /// <summary>
    ///     Gets the message handler logger.
    /// </summary>
    /// <value>The logger.</value>
    protected ILogger<THandler> Logger { get; }

    /// <inheritdoc />
    public virtual async Task<Result> OnBeforeHandling(TMessage message, CancellationToken cancellationToken = default)
        => await Task.FromResult(Result.Try(() =>
        {
            Guard.IsNotNull(message);
        }));

    /// <inheritdoc />
    public virtual async Task<Result> ValidateMessage(TMessage message, CancellationToken cancellationToken = default)
    {
        if (_validator == default)
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
            await _validator.ValidateAndThrowAsync(message, cancellationToken);

            return Result.Ok();
        }
        catch (ValidationException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    /// <inheritdoc />
    public virtual async Task<Result> OnAfterHandling(TMessage message, CancellationToken cancellationToken = default)
        => await Task.FromResult(Result.Try(() =>
        {
            Guard.IsNotNull(message);
        }));
}
