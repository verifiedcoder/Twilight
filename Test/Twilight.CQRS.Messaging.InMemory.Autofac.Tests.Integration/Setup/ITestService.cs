namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;

public interface ITestService
{
    Task Receive(string parameters);
}
