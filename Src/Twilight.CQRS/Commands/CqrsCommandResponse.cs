namespace Twilight.CQRS.Commands;

/// <summary>
///     <para>Represents an encapsulated response from a command handler.</para>
///     <para>Implements <see cref="CqrsMessage" />.</para>
/// </summary>
/// <remarks>
///     <para>
///         Strictly speaking, a command should never return a value, however this response type allows for the scalar
///         return of a command outcome, for example the rows affected for a database update or the identifier for a newly
///         created data record.
///     </para>
///     <para>
///         The design of this response intentionally restricts the type of value that can be returned to structs like int
///         and Guid. For more complex objects, the standard Query / Query response should be used.
///     </para>
///     <para>
///         Beware of using this approach with distributed commands. They really have to be true fire-and-forget
///         operations.
///     </para>
/// </remarks>
/// <typeparam name="TPayload">The type of the payload.</typeparam>
/// <param name="payload">The payload.</param>
/// <param name="correlationId">The message correlation identifier.</param>
/// <param name="causationId">
///     The causation identifier. Identifies the command that caused this response to be produced.
///     Optional.
/// </param>
/// <seealso cref="CqrsMessage" />
public class CqrsCommandResponse<TPayload>(
    TPayload payload, 
    string correlationId, 
    string? causationId = null) : CqrsMessage(correlationId, causationId)
    where TPayload : class
{
    /// <summary>
    ///     Gets the typed command response payload.
    /// </summary>
    /// <value>The payload.</value>
    public TPayload Payload { get; } = payload;
}
