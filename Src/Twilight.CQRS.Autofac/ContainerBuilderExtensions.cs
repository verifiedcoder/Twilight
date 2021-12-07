using System.Reflection;
using Autofac;
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
    // ReSharper disable once ParameterTypeCanBeEnumerable.Global as desired behaviour
    public static void RegisterAssemblyTypes(this ContainerBuilder builder, Assembly assembly, string[] typeNameEndings)
    {
        if (assembly == null || !typeNameEndings.Any())
        {
            return;
        }

        foreach (var typeNameEnding in typeNameEndings)
        {
            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.Name.EndsWith(typeNameEnding, StringComparison.InvariantCultureIgnoreCase) && !t.IsAbstract)
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
        if (!assemblies.Any())
        {
            return builder;
        }

        foreach (var assembly in assemblies)
        {
            builder.RegisterCqrs(assembly);
        }

        return builder;
    }

    /// <summary>
    ///     Registers CQRS command, event, query handlers and message validators in the specified assembly.
    /// </summary>
    /// <param name="builder">The container builder.</param>
    /// <param name="assembly">The assembly to scan.</param>
    public static ContainerBuilder RegisterCqrs(this ContainerBuilder builder, Assembly assembly)
    {
        if (assembly == null)
        {
            return builder;
        }

        builder.RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(ICommandHandler<>));

        builder.RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(ICommandHandler<,>));

        builder.RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(IQueryHandler<,>));

        builder.RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(IEventHandler<>));

        builder.RegisterAssemblyTypes(assembly, new[] { "validator" });

        return builder;
    }
}
