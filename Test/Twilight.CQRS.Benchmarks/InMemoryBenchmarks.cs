using Autofac;
using BenchmarkDotNet.Attributes;
using Twilight.CQRS.Benchmarks.Commands;
using Twilight.CQRS.Benchmarks.Events;
using Twilight.CQRS.Benchmarks.Queries;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Benchmarks;

// Do not seal this class
[MemoryDiagnoser]
[KeepBenchmarkFiles]
public class InMemoryBenchmarks
{
    private IMessageSender _messageSender = null!;

    [GlobalSetup]
    public void Setup()
    {
        var builder = new ContainerBuilder();
        
        builder.RegisterModule<AutofacModule>();
        
        var container = builder.Build();
        
        _messageSender = container.Resolve<IMessageSender>();
    }

    [Benchmark(Description = "Send 1000 Commands")]
    [Arguments(1000)]
    public async Task SendCommands(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var parameters = new MessageParameters("CqrsCommand");
            var command = new SendCqrsCommand(parameters, Guid.NewGuid().ToString());
            
            await _messageSender.Send(command).ConfigureAwait(false);
        }
    }

    [Benchmark(Description = "Send 1000 Queries")]
    [Arguments(1000)]
    public async Task SendQueries(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var parameters = new MessageParameters("CqrsQuery");
            var query = new SendCqrsQuery(parameters, Guid.NewGuid().ToString());
            
            await _messageSender.Send(query).ConfigureAwait(false);
        }
    }

    [Benchmark(Description = "Publish 1000 Events")]
    [Arguments(1000)]
    public async Task PublishEvents(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var parameters = new MessageParameters("CqrsEvent");
            var @event = new SendCqrsEvent(parameters, Guid.NewGuid().ToString());
            
            await _messageSender.Publish(@event).ConfigureAwait(false);
        }
    }
}
