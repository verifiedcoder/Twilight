using BenchmarkDotNet.Running;

// Run all benchmarks by default, or pass BenchmarkDotNet options after `--`.
var summaries = await BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
    .RunAsync(args.Length == 0 ? ["--filter", "*"] : args);

return summaries.Any(summary => summary.HasCriticalValidationErrors
                                || summary.Reports.Any(report => !report.Success)) ? 1 : 0;
