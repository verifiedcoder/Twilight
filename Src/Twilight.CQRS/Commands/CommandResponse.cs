using Microsoft.Toolkit.Diagnostics;

namespace Twilight.CQRS.Commands;

/// <summary>
///     <para>Represents an encapsulated response from a command handler.</para>
///     <para>Implements <see cref="Message" />.</para>
/// </summary>
/// <remarks>
///     <para>
///         Strictly speaking, a command should never return a value, however this response type allows for the scalar
///         return of a command outcome, for example the rows affected for a database update or the identifier for a newly
///         created data record.
///     </para>
///     <para>
///         The design of this response intentionally restricts the type of value that can be returned to structs like int
///         and Guid. For more complex objects, the standard query / query response should be used.
///     </para>
/// </remarks>
/// <typeparam name="TPayload">The type of the payload.</typeparam>
/// <seealso cref="Message" />
public class CommandResponse<TPayload> : Message
    where TPayload : class
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandResponse{TPayload}" /> class.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="correlationId">The message correlation identifier.</param>
    /// <param name="causationId">
    ///     The causation identifier. Identifies the query that caused this response to be produced.
    ///     Optional.
    /// </param>
    public CommandResponse(TPayload payload, string correlationId, string? causationId = null)
        : base(correlationId, causationId)
    {
        Guard.IsNotNull(payload, nameof(payload));

        Payload = payload;
    }

    /// <summary>
    ///     Gets the typed query response payload.
    /// </summary>
    /// <value>The payload.</value>
    public TPayload Payload { get; }
}

