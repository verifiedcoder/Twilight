using System;
using Twilight.CQRS.Contracts;

namespace Twilight.CQRS.Commands
{
    /// <summary>
    ///     <para>
    ///         Represents an action that <em>does</em> something and may carry parameters as a payload or not. Irrespective of
    ///         whether a command has parameters or not, a command is always a 'fire-and-forget' operation and therefore should
    ///         not return a response.
    ///     </para>
    ///     <para>Implements <see cref="Message" />.</para>
    ///     <para>Implements <see cref="ICommand" />.</para>
    /// </summary>
    /// <seealso cref="Message" />
    /// <seealso cref="ICommand" />
    public class Command : Message, ICommand
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Command" /> class.
        /// </summary>
        /// <param name="correlationId">The command correlation identifier.</param>
        /// <param name="causationId">
        ///     The causation identifier. Identifies the message that caused this command to be produced.
        ///     Optional.
        /// </param>
        public Command(Guid correlationId, Guid? causationId = null)
            : base(correlationId, causationId)
        {
        }
    }

    /// <summary>
    ///     <para>
    ///         Represents an action that <em>does</em> something and may carry a payload of arbitrary type
    ///         <typeparamref name="TParameters" />. The command may carry parameters as a payload or not. Irrespective of
    ///         whether a command has parameters or not, a command is always a 'fire-and-forget' operation and therefore should
    ///         not return a response.
    ///     </para>
    ///     <para>Implements <see cref="Message" />.</para>
    ///     <para>Implements <see cref="ICommand" />.</para>
    /// </summary>
    /// <typeparam name="TParameters">The type of the parameters.</typeparam>
    /// <seealso cref="Message" />
    /// <seealso cref="ICommand" />
    public class Command<TParameters> : Message, ICommand
        where TParameters : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Command{TParameters}" /> class.
        /// </summary>
        /// <param name="parameters">The typed command parameters.</param>
        /// <param name="correlationId">The command correlation identifier.</param>
        /// <param name="causationId">
        ///     The causation identifier. Identifies the message that caused this command to be produced.
        ///     Optional.
        /// </param>
        public Command(TParameters parameters, Guid correlationId, Guid? causationId = null)
            : base(correlationId, causationId) => Params = parameters;

        /// <summary>
        ///     Gets the typed command parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public TParameters Params { get; }
    }
}
