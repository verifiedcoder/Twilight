using Microsoft.EntityFrameworkCore;
using Twilight.Samples.CQRS.Data.Entities;

namespace Twilight.Samples.CQRS.Data;

public sealed class SampleDataContext : DbContext
{
    public SampleDataContext(DbContextOptions<SampleDataContext> options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; } = null!;
}
