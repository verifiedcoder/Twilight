using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Diagnostics;
using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS;

/// <inheritdoc />
public abstract class CqrsMessageHandler<THandler, TMessage> : ICqrsMessageHandler<TMessage>
    where TMessage : class
{
    private const string DefaultAssemblyVersion = "1.0.0.0";

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
    ///     Gets the Open Telemetry activity source identifier.
    /// </summary>
    /// <value>The activity source identifier.</value>
    internal string ActivitySourceName => typeof(CqrsMessageHandler<THandler, TMessage>).Namespace ?? nameof(CqrsMessageHandler<THandler, TMessage>);

    /// <summary>
    ///     Gets the assembly version.
    /// </summary>
    /// <value>The assembly version.</value>
#pragma warning disable CA1822 // Mark members as a static field in a generic type is not shared among instances of different close constructed types.
    internal string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? DefaultAssemblyVersion;
#pragma warning restore CA1822 // Mark members as static

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
