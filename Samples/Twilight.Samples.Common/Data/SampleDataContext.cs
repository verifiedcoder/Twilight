using Microsoft.EntityFrameworkCore;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Data;

public sealed class SampleDataContext(DbContextOptions<SampleDataContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; init; } = null!;
}
