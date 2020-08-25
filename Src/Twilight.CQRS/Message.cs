using System;
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
        protected Message(Guid correlationId, Guid? causationId = null)
        {
            MessageId = SequentialGuid.NewGuid();
            CorrelationId = correlationId;
            CausationId = causationId;
        }

        /// <inheritdoc />
        public Guid MessageId { get; }

        /// <inheritdoc />
        public Guid CorrelationId { get; }

        /// <inheritdoc />
        public Guid? CausationId { get; }
    }
}
