namespace Twilight.CQRS.Interfaces;

/// <summary>
///     <para>Represents a message of type query with a response of arbitrary type.</para>
///     <para>Implements <see cref="ICqrsMessage" />.</para>
/// </summary>
/// <typeparam name="TResponse">The type of the response payload.</typeparam>
/// <seealso cref="ICqrsMessage" />
public interface ICqrsQuery<TResponse> : ICqrsMessage
{
}
