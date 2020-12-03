using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using FluentAssertions;
using Twilight.CQRS.Autofac.Tests.Unit.Setup;
using Xunit;

namespace Twilight.CQRS.Autofac.Tests.Unit
{
    public sealed class CqrsRegistrationExtensionsTests
    {
        private readonly ContainerBuilder _builder;
        public CqrsRegistrationExtensionsTests() => _builder = new ContainerBuilder();

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

        [Fact]
        public void CallingRegisterForCqrsWithNullAssembliesDoesNotThrow()
        {
            Action subjectResult = () => { _builder.RegisterAssemblyTypes(); };

            subjectResult.Should().NotThrow();
        }

        [Fact]
        public void RegisterForCqrsRegistersAssemblyServices()
        {
            var assembly = typeof(TestCommandHandler).Assembly;

            _builder.RegisterAssemblyTypes(assembly, new[] {"Handler"});

            var container = _builder.Build();

            container.ComponentRegistry.Registrations.Count().Should().Be(2);
            container.ComponentRegistry.Registrations.Should().OnlyHaveUniqueItems();

            var services = (from r in container.ComponentRegistry.Registrations
                            from s in r.Services
                            select s.Description).ToList();

            var expectedServices = new List<string>
                                   {
                                       typeof(TestCommandHandler).Namespace ?? string.Empty
                                   };

            AssertOnExpectedServices(expectedServices, services);
        }
    }
}
