namespace Twilight.CQRS.Benchmarks;

internal static class BenchmarkValidation
{
    // A failed dispatch must stop the run instead of producing misleading timings.
    public static void EnsureSuccess(ResultBase result)
    {
        if (result.IsFailed)
        {
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(error => error.Message)));
        }
    }
}
