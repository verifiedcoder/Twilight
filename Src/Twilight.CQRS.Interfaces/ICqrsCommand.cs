using System.Diagnostics.CodeAnalysis;

namespace Twilight.CQRS.Interfaces;

/// <summary>
///     <para>Represents a message of type command.</para>
///     <para>Implements <see cref="ICqrsMessage" />.</para>
/// </summary>
/// <seealso cref="ICqrsMessage" />
public interface ICqrsCommand : ICqrsMessage;

/// <summary>
///     <para>Represents a message of type command with a response of arbitrary type.</para>
///     <para>Implements <see cref="ICqrsMessage" />.</para>
/// </summary>
/// <typeparam name="TResponse">The type of the response payload.</typeparam>
/// <seealso cref="ICqrsMessage" />
[SuppressMessage("Design", "S2326: Intentional design.")]

public interface ICqrsCommand<TResponse> : ICqrsMessage;
