using FluentResults;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;

public interface ITestService
{
    Task<Result> Receive(string parameters);
}
