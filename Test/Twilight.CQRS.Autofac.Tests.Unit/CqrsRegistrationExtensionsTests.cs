using Autofac;
using FluentAssertions;
using Twilight.CQRS.Autofac.Tests.Unit.Setup;
using Xunit;

namespace Twilight.CQRS.Autofac.Tests.Unit;

public sealed class CqrsRegistrationExtensionsTests
{
    private readonly ContainerBuilder _builder = new();

    // Setup

    [Fact]
    public void CallingRegisterForCqrsWithNullAssembliesDoesNotThrow()
    {
        // Arrange / Act
        var subjectResult = () => { _builder.RegisterAssemblyTypes(); };

        // Assert
        subjectResult.Should().NotThrow();
    }

    [Fact]
    public void RegisterForCqrsRegistersAssemblyServices()
    {
        // Arrange
        var assembly = typeof(TestCqrsCommandHandler).Assembly;

        _builder.RegisterAssemblyTypes(assembly);

        // Act
        var container = _builder.Build();

        // Assert
        container.ComponentRegistry.Registrations.Count().Should().Be(4);
        container.ComponentRegistry.Registrations.Should().OnlyHaveUniqueItems();

        var services = (from r in container.ComponentRegistry.Registrations
                        from s in r.Services
                        select s.Description).ToList();

        var expectedServices = new List<string>
        {
            typeof(TestCqrsCommandHandler).Namespace ?? string.Empty
        };

        AssertOnExpectedServices(expectedServices, services);
    }

    // ReSharper disable once SuggestBaseTypeForParameter as don't want multiple enumeration
    private static void AssertOnExpectedServices(IEnumerable<string> expectedServices, List<string> services)
    {
        foreach (var expectedService in expectedServices)
        {
            var selectedService = (from service in services
                                   where service.Contains(expectedService)
                                   select service).FirstOrDefault();

            selectedService.Should().NotBeNull();
        }
    }
}
