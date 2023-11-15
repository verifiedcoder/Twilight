namespace Twilight.CQRS.Messaging.Common;

/// <summary>
///     <para>
///         Exception that should be thrown when multiple command handlers for a specific command are found. This class
///         cannot be inherited.
///     </para>
///     <para>Implements <see cref="Exception" />.</para>
/// </summary>
/// <seealso cref="Exception" />
public sealed class MultipleCommandHandlersDefinedException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MultipleCommandHandlersDefinedException" /> class.
    /// </summary>
    /// <param name="typeName">Name of the type.</param>
    public MultipleCommandHandlersDefinedException(string typeName)
        : base($"Multiple command handlers for type '{typeName}' were found. Only one handler can exist for a command.")
    {
    }
}
