using Microsoft.Extensions.Logging;

namespace Twilight.CQRS.Messaging.InMemory.Autofac;

/// <summary>
///     High-performance logging extension methods for <see cref="AutofacInMemoryMessageSender" />. 
///     Uses LoggerMessage source generator to eliminate runtime overhead.
/// </summary>
internal static partial class AutofacInMemoryMessageSenderLogs
{
    /// <summary>
    ///     Logs a warning when no events are received for publishing.
    /// </summary>
    [LoggerMessage(LogLevel.Warning, "No events received for publishing when at least one event was expected. Check calls to publish.")]
    public static partial void LogNoEventsReceived(this ILogger logger);

    /// <summary>
    ///     Logs a critical error when multiple handlers are found for a command.
    /// </summary>
    [LoggerMessage(LogLevel.Critical, "Multiple handlers found in {AssemblyQualifiedName}.")]
    public static partial void LogMultipleHandlersFound(this ILogger logger, string assemblyQualifiedName);

    /// <summary>
    ///     Logs a critical error when handler resolution fails.
    /// </summary>
    [LoggerMessage(LogLevel.Critical, "No concrete handlers for type '{AssemblyQualifiedName}' could be resolved.")]
    public static partial void LogHandlerResolutionFailure(this ILogger logger, string assemblyQualifiedName, Exception ex);

    /// <summary>
    ///     Logs a critical error when a component is not found in the DI container.
    /// </summary>
    [LoggerMessage(LogLevel.Critical, "No concrete handlers for type '{AssemblyQualifiedName}' could be found.")]
    public static partial void LogComponentNotFound(this ILogger logger, string assemblyQualifiedName, Exception ex);

    /// <summary>
    ///     Logs a critical error when a handler is not found.
    /// </summary>
    [LoggerMessage(LogLevel.Critical, "Handler not found in '{AssemblyQualifiedName}'.")]
    public static partial void LogHandlerNotFoundCritical(this ILogger logger, string assemblyQualifiedName);

    /// <summary>
    ///     Logs a critical error when handler execution fails.
    /// </summary>
    [LoggerMessage(LogLevel.Critical, "Could not execute handler for '{AssemblyQualifiedName}'.")]
    public static partial void LogHandlerExecutionFailure(this ILogger logger, string assemblyQualifiedName, Exception ex);
}
