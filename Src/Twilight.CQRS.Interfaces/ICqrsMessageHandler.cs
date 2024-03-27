using FluentResults;

namespace Twilight.CQRS.Interfaces;

/// <summary>
///     Represents base message handling functionality. This class cannot be inherited.
/// </summary>
/// <typeparam name="TMessage">The type of the message.</typeparam>
public interface ICqrsMessageHandler<in TMessage>
{
    /// <summary>
    ///     Occurs before handling a message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> OnBeforeHandling(TMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Occurs when validating a message.
    /// </summary>
    /// <param name="message">The message to be validated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> ValidateMessage(TMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Occurs when handling a message has completed.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Result> OnAfterHandling(TMessage message, CancellationToken cancellationToken = default);
}
