using System;

namespace Twilight.CQRS.Contracts
{
    /// <summary>
    ///     <para>Represents base properties for messages. This class cannot be instantiated.</para>
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        ///     Gets the message identifier.
        /// </summary>
        /// <value>The message identifier.</value>
        Guid MessageId { get; }

        /// <summary>
        ///     Gets the correlation identifier.
        /// </summary>
        /// <value>The message correlation identifier.</value>
        Guid CorrelationId { get; }

        /// <summary>
        ///     Gets the causation identifier.
        /// </summary>
        /// <remarks>Identifies the message (by that message's identifier) that caused a message instance to be produced.</remarks>
        /// <value>The causation identifier.</value>
        Guid? CausationId { get; }
    }
}
