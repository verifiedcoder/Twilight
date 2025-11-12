namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup;

public interface IVerifier
{
    Task<Result> Receive(string parameter);
}
