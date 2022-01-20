using Autofac;
using BenchmarkDotNet.Attributes;
using Twilight.CQRS.Benchmarks.Commands;
using Twilight.CQRS.Benchmarks.Events;
using Twilight.CQRS.Benchmarks.Queries;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Benchmarks;

[MemoryDiagnoser]
[KeepBenchmarkFiles]
public class InMemoryBenchmarks
{
    private readonly IMessageSender _messageSender;

    public InMemoryBenchmarks()
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule<AutofacModule>();

        var container = builder.Build();

        _messageSender = container.Resolve<IMessageSender>();
    }

    [Benchmark(Description = nameof(SendCommand))]
    public void SendCommand()
    {
        for (var i = 0; i < 5000; i++)
        {
            var parameters = new MessageParameters("CqrsCommand");
            var command = new SendCqrsCommand(parameters, Guid.NewGuid().ToString());

            _messageSender.Send(command);
        }
    }

    [Benchmark(Description = nameof(SendQuery))]
    public void SendQuery()
    {
        for (var i = 0; i < 5000; i++)
        {
            var parameters = new MessageParameters("CqrsQuery");
            var query = new SendCqrsQuery(parameters, Guid.NewGuid().ToString());

            _ = _messageSender.Send(query);
        }
    }

    [Benchmark(Description = nameof(PublishEvent))]
    public void PublishEvent()
    {
        for (var i = 0; i < 10000; i++)
        {
            var parameters = new MessageParameters("CqrsEvent");
            var @event = new SendCqrsEvent(parameters, Guid.NewGuid().ToString());

            _messageSender.Publish(@event);
        }
    }
}
