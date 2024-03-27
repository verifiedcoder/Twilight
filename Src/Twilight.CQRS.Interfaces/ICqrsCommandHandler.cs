using FluentResults;

namespace Twilight.CQRS.Interfaces;

/// <summary>
///     Represents a means of handling a command in order to broker a result.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
public interface ICqrsCommandHandler<in TCommand> : ICqrsMessageHandler<TCommand>
    where TCommand : ICqrsCommand
{
    /// <summary>
    ///     Handles the command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous command handler operation.</returns>
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
///     Represents a command message handler.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface ICqrsCommandHandler<in TCommand, TResponse> : ICqrsMessageHandler<TCommand>
    where TCommand : class, ICqrsCommand<TResponse>
    where TResponse : class
{
    /// <summary>
    ///     Handles the command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     <para>A task that represents the asynchronous command handler operation.</para>
    ///     <para>The task result contains the command execution response.</para>
    /// </returns>
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default);
}
