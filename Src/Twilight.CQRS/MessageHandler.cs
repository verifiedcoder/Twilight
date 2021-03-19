using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
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
        protected MessageHandler(IValidator<TMessage>? validator = default) => _validator = validator;

        /// <summary>
        ///     Gets the Open Telemetry activity source identifier.
        /// </summary>
        /// <value>The activity source identifier.</value>
        internal static string ActivitySourceName => typeof(MessageHandler<TMessage>).Namespace ?? nameof(MessageHandler<TMessage>);

        /// <summary>
        ///     Gets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        internal static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";

        /// <inheritdoc />
        public virtual async Task OnBeforeHandling(TMessage message, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual async Task ValidateMessage(TMessage message, CancellationToken cancellationToken = default)
        {
            if (_validator == default)
            {
                return;
            }

            await _validator.ValidateAndThrowAsync(message, cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task OnAfterHandling(TMessage message, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
