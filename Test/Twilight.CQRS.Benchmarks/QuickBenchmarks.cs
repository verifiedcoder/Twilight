using Autofac;
using BenchmarkDotNet.Attributes;
using Twilight.CQRS.Benchmarks.Commands;
using Twilight.CQRS.Benchmarks.Events;
using Twilight.CQRS.Benchmarks.Queries;
using Twilight.CQRS.Messaging.Interfaces;

namespace Twilight.CQRS.Benchmarks;

// Do not seal this class
[MemoryDiagnoser]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
[SimpleJob(warmupCount: 1, iterationCount: 3, invocationCount: 100)]
public class QuickBenchmarks
{
    private IMessageSender _messageSender = null!;
    private SendCqrsCommand _command = null!;
    private SendCqrsQuery _query = null!;
    private SendCqrsEvent _event = null!;

    [GlobalSetup]
    public void Setup()
    {
        var builder = new ContainerBuilder();
        
        builder.RegisterModule<AutofacModule>();
        
        var container = builder.Build();
        
        _messageSender = container.Resolve<IMessageSender>();

        // Pre-create objects to avoid allocation noise
        var commandParams = new MessageParameters("CqrsCommand");
        
        _command = new SendCqrsCommand(commandParams, Guid.NewGuid().ToString());

        var queryParams = new MessageParameters("CqrsQuery");
        
        _query = new SendCqrsQuery(queryParams, Guid.NewGuid().ToString());

        var eventParams = new MessageParameters("CqrsEvent");
        
        _event = new SendCqrsEvent(eventParams, Guid.NewGuid().ToString());
    }

    [Benchmark(Description = "Send Command")]
    public async Task SendCommand()
        => await _messageSender.Send(_command).ConfigureAwait(false);

    [Benchmark(Description = "Send Query")]
    public async Task SendQuery() 
        => await _messageSender.Send(_query).ConfigureAwait(false);

    [Benchmark(Description = "Publish Event")]
    public async Task PublishEvent() 
        => await _messageSender.Publish(_event).ConfigureAwait(false);
}