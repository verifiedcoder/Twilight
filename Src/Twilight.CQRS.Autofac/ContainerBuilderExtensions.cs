using System.Reflection;
using Autofac;
using Microsoft.Toolkit.Diagnostics;
using Twilight.CQRS.Interfaces;

namespace Twilight.CQRS.Autofac;

/// <summary>
///     Provides extensions to the Autofac Container builder that allow for the easy registration of CQRS components from
///     assemblies.
/// </summary>
public static class ContainerBuilderExtensions
{
    /// <summary>
    ///     Scans the specified assembly and registers types matching the specified endings against their implemented
    ///     interfaces.
    /// </summary>
    /// <remarks>All registrations will be made with instance per lifetime scope.</remarks>
    /// <param name="builder">The container builder.</param>
    /// <param name="assembly">The assembly to scan.</param>
    /// <param name="typeNameEndings">The file endings to match against.</param>
    public static void RegisterAssemblyTypes(this ContainerBuilder builder, Assembly assembly, string[] typeNameEndings)
    {
        Guard.IsNotNull(assembly, nameof(assembly));
        Guard.IsNotNull(typeNameEndings, nameof(typeNameEndings));

        if (!typeNameEndings.Any())
        {
            return;
        }

        foreach (var typeNameEnding in typeNameEndings)
        {
            builder.RegisterAssemblyTypes(assembly)
                   .Where(type => type.Name.EndsWith(typeNameEnding, StringComparison.InvariantCultureIgnoreCase) && !type.IsAbstract)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope()
                   .AsSelf();
        }
    }

    /// <summary>
    ///     Registers CQRS command, event, query handlers and message validators in the specified assembly.
    /// </summary>
    /// <param name="builder">The container builder.</param>
    /// <param name="assemblies">The assemblies to scan.</param>
    public static ContainerBuilder RegisterCqrs(this ContainerBuilder builder, IEnumerable<Assembly> assemblies)
    {
        Guard.IsNotNull(assemblies, nameof(assemblies));

        var assemblyList = assemblies.ToList();

        if (!assemblyList.Any())
        {
            return builder;
        }

        assemblyList.ForEach(assembly => builder.RegisterCqrs(assembly));

        return builder;
    }

    /// <summary>
    ///     Registers CQRS command, event, query handlers and message validators in the specified assembly.
    /// </summary>
    /// <param name="builder">The container builder.</param>
    /// <param name="assembly">The assembly to scan.</param>
    public static ContainerBuilder RegisterCqrs(this ContainerBuilder builder, Assembly assembly)
    {
        Guard.IsNotNull(assembly, nameof(assembly));

        builder.RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(ICqrsCommandHandler<>));

        builder.RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(ICqrsCommandHandler<,>));

        builder.RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(ICqrsQueryHandler<,>));

        builder.RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(ICqrsEventHandler<>));

        builder.RegisterAssemblyTypes(assembly, new[] { "validator" });

        return builder;
    }
}
