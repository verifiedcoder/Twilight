using BenchmarkDotNet.Running;
using Twilight.CQRS.Benchmarks;

// Run in Release Build Configuration
var summary = BenchmarkRunner.Run<InMemoryBenchmarks>();

Console.WriteLine(summary);
Console.ReadLine();

Environment.Exit(0);
