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
    private IContainer _container = null!;
    private IMessageSender _messageSender = null!;

    [BenchmarkCancellation]
    public CancellationToken CancellationToken { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var builder = new ContainerBuilder();
        
        builder.RegisterModule<AutofacModule>();
        
        _container = builder.Build();
        
        _messageSender = _container.Resolve<IMessageSender>();
    }

    [GlobalCleanup]
    public void Cleanup() => _container.Dispose();

    [Benchmark(Description = "Send 1000 Commands")]
    [Arguments(1000)]
    public async Task SendCommands(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var parameters = new MessageParameters("CqrsCommand");
            var command = new SendCqrsCommand(parameters, Guid.NewGuid().ToString());
            
            BenchmarkValidation.EnsureSuccess(await _messageSender.Send(command, CancellationToken).ConfigureAwait(false));
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
            
            BenchmarkValidation.EnsureSuccess(await _messageSender.Send(query, CancellationToken).ConfigureAwait(false));
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
            
            BenchmarkValidation.EnsureSuccess(await _messageSender.Publish(@event, CancellationToken).ConfigureAwait(false));
        }
    }
}
