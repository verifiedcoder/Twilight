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
[ShortRunJob]
public class QuickBenchmarks
{
    private IContainer _container = null!;
    private IMessageSender _messageSender = null!;
    private SendCqrsCommand _command = null!;
    private SendCqrsQuery _query = null!;
    private SendCqrsEvent _event = null!;

    [BenchmarkCancellation]
    public CancellationToken CancellationToken { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var builder = new ContainerBuilder();
        
        builder.RegisterModule<AutofacModule>();
        
        _container = builder.Build();
        
        _messageSender = _container.Resolve<IMessageSender>();

        // Pre-create objects to avoid allocation noise
        var commandParams = new MessageParameters("CqrsCommand");
        
        _command = new SendCqrsCommand(commandParams, Guid.NewGuid().ToString());

        var queryParams = new MessageParameters("CqrsQuery");
        
        _query = new SendCqrsQuery(queryParams, Guid.NewGuid().ToString());

        var eventParams = new MessageParameters("CqrsEvent");
        
        _event = new SendCqrsEvent(eventParams, Guid.NewGuid().ToString());
    }

    [GlobalCleanup]
    public void Cleanup() => _container.Dispose();

    [Benchmark(Description = "Send Command")]
    public async Task SendCommand()
        => BenchmarkValidation.EnsureSuccess(await _messageSender.Send(_command, CancellationToken).ConfigureAwait(false));

    [Benchmark(Description = "Send Query")]
    public async Task SendQuery() 
        => BenchmarkValidation.EnsureSuccess(await _messageSender.Send(_query, CancellationToken).ConfigureAwait(false));

    [Benchmark(Description = "Publish Event")]
    public async Task PublishEvent() 
        => BenchmarkValidation.EnsureSuccess(await _messageSender.Publish(_event, CancellationToken).ConfigureAwait(false));
}
