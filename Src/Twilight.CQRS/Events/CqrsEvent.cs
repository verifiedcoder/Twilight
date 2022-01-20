using Microsoft.Toolkit.Diagnostics;
using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS.Events;

/// <summary>
///     <para>
///         Represents something that has already taken place in the domain. As such, always name an event with a
///         past-participle verb, e.g. UserCreated. Events are facts and can be used to influence business decisions within
///         the domain. Irrespective of whether an event has parameters or not, an event is always a 'fire-and-forget'
///         operation and therefore does not return a response.
///     </para>
///     <para>Implements <see cref="CqrsMessage" />.</para>
///     <para>Implements <see cref="ICqrsEvent" />.</para>
/// </summary>
/// <seealso cref="CqrsMessage" />
/// <seealso cref="ICqrsEvent" />
public class CqrsEvent : CqrsMessage, ICqrsEvent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsEvent" /> class.
    /// </summary>
    /// <param name="correlationId">The event correlation identifier.</param>
    /// <param name="causationId">
    ///     The causation identifier. Identifies the message that caused this event to be produced.
    ///     Optional.
    /// </param>
    public CqrsEvent(string correlationId, string? causationId = null)
        : base(correlationId, causationId)
    {
    }
}

/// <summary>
///     <para>
///         Represents something that has already taken place in the domain. As such, always name an event with a
///         past-participle verb, e.g. UserCreated. Events are facts and can be used to influence business decisions within
///         the domain. Irrespective of whether an event has parameters or not, an event is always a 'fire-and-forget'
///         operation and therefore does not return a response.
///     </para>
///     <para>Implements <see cref="CqrsMessage" />.</para>
///     <para>Implements <see cref="ICqrsEvent" />.</para>
/// </summary>
/// <typeparam name="TParameters">The type of the parameters.</typeparam>
/// <seealso cref="CqrsMessage" />
/// <seealso cref="CqrsEvent" />
public class CqrsEvent<TParameters> : CqrsMessage, ICqrsEvent
    where TParameters : class
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsEvent{TParameters}" /> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="correlationId">The event correlation identifier.</param>
    /// <param name="causationId">
    ///     The causation identifier. Identifies the message that caused this event to be produced.
    ///     Optional.
    /// </param>
    public CqrsEvent(TParameters parameters, string correlationId, string? causationId = null)
        : base(correlationId, causationId)
    {
        Guard.IsNotNull(parameters, nameof(parameters));

        Params = parameters;
    }

    /// <summary>
    ///     Gets the typed event parameters.
    /// </summary>
    /// <value>The parameters.</value>
    public TParameters Params { get; }
}
