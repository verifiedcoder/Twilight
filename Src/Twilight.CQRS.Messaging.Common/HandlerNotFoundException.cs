using System.Runtime.Serialization;

namespace Twilight.CQRS.Messaging.Common;

/// <summary>
///     <para>Exception that should be thrown when a message handler is not found. This class cannot be inherited.</para>
///     <para>Implements <see cref="Exception" />.</para>
/// </summary>
/// <seealso cref="Exception" />
[Serializable]
public sealed class HandlerNotFoundException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HandlerNotFoundException" /> class.
    /// </summary>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="exception">The exception.</param>
    public HandlerNotFoundException(string typeName, Exception? exception = null)
        : base($"No concrete handlers for type '{typeName}' could be found.", exception)
    {
    }

    /// <inheritdoc />
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
        => base.GetObjectData(info, context);
}
