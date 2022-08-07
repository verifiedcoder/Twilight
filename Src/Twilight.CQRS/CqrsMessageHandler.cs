using FluentValidation;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Diagnostics;
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
        Guard.IsNotNull(logger, nameof(logger));

        _validator = validator;

        Logger = logger;
    }

    /// <summary>
    ///     Gets the message handler logger.
    /// </summary>
    /// <value>The logger.</value>
    protected ILogger<THandler> Logger { get; }

    /// <inheritdoc />
    public virtual async Task OnBeforeHandling(TMessage message, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(message, nameof(message));

        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual async Task ValidateMessage(TMessage message, CancellationToken cancellationToken = default)
    {
        if (_validator == default)
        {
            return;
        }

        Guard.IsNotNull(message, nameof(message));

        await _validator.ValidateAndThrowAsync(message, cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task OnAfterHandling(TMessage message, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(message, nameof(message));

        await Task.CompletedTask;
    }
}
