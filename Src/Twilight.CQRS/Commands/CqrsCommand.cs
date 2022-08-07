using CommunityToolkit.Diagnostics;
using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS.Commands;

/// <summary>
///     <para>
///         Represents an action that <em>does</em> something and may carry parameters as a payload or not. Irrespective of
///         whether a command has parameters or not, a command is always a 'fire-and-forget' operation and therefore should
///         not return a response.
///     </para>
///     <para>Implements <see cref="CqrsMessage" />.</para>
///     <para>Implements <see cref="ICqrsCommand" />.</para>
/// </summary>
/// <seealso cref="CqrsMessage" />
/// <seealso cref="ICqrsCommand" />
public class CqrsCommand : CqrsMessage, ICqrsCommand
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsCommand" /> class.
    /// </summary>
    /// <param name="correlationId">The command correlation identifier.</param>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="causationId">
    ///     The causation identifier. Identifies the message that caused this command to be produced.
    ///     Optional.
    /// </param>
    public CqrsCommand(string correlationId, string? causationId = null, string? sessionId = null)
        : base(correlationId, sessionId, causationId)
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
///     <para>Implements <see cref="CqrsMessage" />.</para>
///     <para>Implements <see cref="ICqrsCommand" />.</para>
/// </summary>
/// <typeparam name="TParameters">The type of the parameters.</typeparam>
/// <seealso cref="CqrsMessage" />
/// <seealso cref="ICqrsCommand" />
public class CqrsCommand<TParameters> : CqrsMessage, ICqrsCommand
    where TParameters : class
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsCommand{TParameters}" /> class.
    /// </summary>
    /// <param name="parameters">The typed command parameters.</param>
    /// <param name="correlationId">The command correlation identifier.</param>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="causationId">
    ///     The causation identifier. Identifies the message that caused this command to be produced.
    ///     Optional.
    /// </param>
    public CqrsCommand(TParameters parameters, string correlationId, string? sessionId = null, string? causationId = null)
        : base(correlationId, sessionId, causationId)
    {
        Guard.IsNotNull(parameters, nameof(parameters));

        Params = parameters;
    }

    /// <summary>
    ///     Gets the typed command parameters.
    /// </summary>
    /// <value>The parameters.</value>
    public TParameters Params { get; }
}

/// <summary>
///     <para>
///         Represents a result and does not change the observable state of the system (i.e. is free of side-effects).
///     </para>
///     <para>Implements <see cref="CqrsMessage" />.</para>
///     <para>Implements <see cref="ICqrsCommand" />.</para>
/// </summary>
/// <typeparam name="TParameters">The type of the parameters.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <seealso cref="CqrsMessage" />
/// <seealso cref="ICqrsCommand" />
public class CqrsCommand<TParameters, TResponse> : CqrsMessage, ICqrsCommand<TResponse>
    where TParameters : class
    where TResponse : class
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsCommand{TParameters, TResponse}" /> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="correlationId">The command correlation identifier.</param>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="causationId">
    ///     The causation identifier. Identifies the message that caused this command to be produced.
    ///     Optional.
    /// </param>
    public CqrsCommand(TParameters parameters, string correlationId, string? sessionId = null, string? causationId = null)
        : base(correlationId, sessionId, causationId)
    {
        Guard.IsNotNull(parameters, nameof(parameters));

        Params = parameters;
    }

    /// <summary>
    ///     Gets the typed command parameters.
    /// </summary>
    /// <value>The parameters.</value>
    public TParameters Params { get; }
}
