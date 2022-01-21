namespace Twilight.CQRS.Exceptions;

/// <summary>
///     Exception thrown when a handler encounters and issue.
/// </summary>
[Serializable]
public sealed class HandlerException : Exception
{
    /// <summary>
    ///     Initializes a new instance of <see cref="HandlerException" />.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public HandlerException(string? message)
        : base(message)
    {
    }
}
