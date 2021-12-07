namespace Twilight.CQRS.Interfaces;

/// <summary>
///     <para>Represents a message of type command.</para>
///     <para>Implements <see cref="IMessage" />.</para>
/// </summary>
/// <seealso cref="IMessage" />
public interface ICommand : IMessage
{
}

/// <summary>
///     <para>Represents a message of type command with a response of arbitrary type.</para>
///     <para>Implements <see cref="IMessage" />.</para>
/// </summary>
/// <typeparam name="TResponse">The type of the response payload.</typeparam>
/// <seealso cref="IMessage" />
public interface ICommand<TResponse> : IMessage
{
}
