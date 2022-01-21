using Microsoft.EntityFrameworkCore;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Data;

public sealed class SampleDataContext : DbContext
{
    public SampleDataContext(DbContextOptions<SampleDataContext> options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; } = null!;
}
