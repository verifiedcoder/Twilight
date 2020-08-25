using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Twilight.CQRS.Contracts;

namespace Twilight.CQRS
{
    /// <inheritdoc />
    public abstract class MessageHandler<TMessage> : IMessageHandler<TMessage>
    {
        private readonly IValidator<TMessage>? _validator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageHandler{TMessage}" /> class.
        /// </summary>
        /// <param name="validator">The message validator.</param>
        /// <param name="logger">The logger.</param>
        protected MessageHandler(ILogger<IMessageHandler<TMessage>> logger, IValidator<TMessage>? validator = default)
        {
            Logger = logger;

            _validator = validator;

        }

        /// <inheritdoc />
        public ILogger<IMessageHandler<TMessage>> Logger { get; }

        /// <inheritdoc />
        public virtual async Task OnBeforeHandling(TMessage message, CancellationToken cancellationToken = default)
        {
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"[{nameof(OnBeforeHandling)}] {message?.GetType()}");
            }

            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual async Task ValidateMessage(TMessage message, CancellationToken cancellationToken = default)
        {
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"[{nameof(ValidateMessage)}] {message?.GetType()}");
            }

            if (_validator == default)
            {
                return;
            }

            await _validator.ValidateAndThrowAsync(message, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task OnAfterHandling(TMessage message, CancellationToken cancellationToken = default)
        {
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"[{nameof(OnAfterHandling)}] {message?.GetType()}");
            }

            await Task.CompletedTask;
        }
    }
}
