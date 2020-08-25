namespace Twilight.CQRS.Contracts
{
    /// <summary>
    ///     <para>Represents a message of type query with a response of arbitrary type.</para>
    ///     <para>Implements <see cref="IMessage" />.</para>
    /// </summary>
    /// <typeparam name="TResponse">The type of the response payload.</typeparam>
    /// <seealso cref="IMessage" />
    public interface IQuery<TResponse> : IMessage
    {
    }
}
