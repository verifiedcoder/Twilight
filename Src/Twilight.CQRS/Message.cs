using Taikandi;
using Twilight.CQRS.Contracts;

namespace Twilight.CQRS
{
    /// <inheritdoc />
    public abstract class Message : IMessage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="correlationId">The message correlation identifier.</param>
        /// <param name="causationId">
        ///     The causation identifier. Identifies the message that caused this message to be produced.
        ///     Optional.
        /// </param>
        protected Message(string correlationId, string? causationId = null)
        {
            MessageId = SequentialGuid.NewGuid().ToString();
            CorrelationId = correlationId;
            CausationId = causationId;
        }

        /// <inheritdoc />
        public string MessageId { get; }

        /// <inheritdoc />
        public string CorrelationId { get; }

        /// <inheritdoc />
        public string? CausationId { get; }
    }
}
