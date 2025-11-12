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
/// <param name="correlationId">The command correlation identifier.</param>
/// <param name="causationId">
///     The causation identifier. Identifies the message that caused this command to be produced.
///     Optional.
/// </param>
/// <param name="sessionId">The session identifier.</param>
/// <seealso cref="CqrsMessage" />
/// <seealso cref="ICqrsCommand" />
public class CqrsCommand(
    string correlationId,
    string? causationId = null, 
    string? sessionId = null) : CqrsMessage(correlationId, sessionId, causationId), ICqrsCommand;

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
/// <param name="parameters">The typed command parameters.</param>
/// <param name="correlationId">The command correlation identifier.</param>
/// <param name="sessionId">The session identifier.</param>
/// <param name="causationId">
///     The causation identifier. Identifies the message that caused this command to be produced.
///     Optional.
/// </param>
/// <seealso cref="CqrsMessage" />
/// <seealso cref="ICqrsCommand" />
public class CqrsCommand<TParameters>(
    TParameters parameters,
    string correlationId, 
    string? sessionId = null, 
    string? causationId = null) : CqrsMessage(correlationId, sessionId, causationId), ICqrsCommand
    where TParameters : class
{
    /// <summary>
    ///     Gets the typed command parameters.
    /// </summary>
    /// <value>The parameters.</value>
    public TParameters Params { get; } = parameters;
}

/// <summary>
///     <para>
///         Represents a result and does not change the observable state of the system (i.e. is free of side effects).
///     </para>
///     <para>Implements <see cref="CqrsMessage" />.</para>
///     <para>Implements <see cref="ICqrsCommand{TResponse}" />.</para>
/// </summary>
/// <typeparam name="TParameters">The type of the parameters.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <param name="parameters">The parameters.</param>
/// <param name="correlationId">The command correlation identifier.</param>
/// <param name="sessionId">The session identifier.</param>
/// <param name="causationId">
///     The causation identifier. Identifies the message that caused this command to be produced.
///     Optional.
/// </param>
/// <seealso cref="CqrsMessage" />
/// <seealso cref="ICqrsCommand{TResponse}" />
public class CqrsCommand<TParameters, TResponse>(
    TParameters parameters,
    string correlationId,
    string? sessionId = null,
    string? causationId = null) : CqrsMessage(correlationId, sessionId, causationId), ICqrsCommand<TResponse>
    where TParameters : class
    where TResponse : class
{
    /// <summary>
    ///     Gets the typed command parameters.
    /// </summary>
    /// <value>The parameters.</value>
    public TParameters Params { get; } = parameters;
}
