using Microsoft.EntityFrameworkCore;
using Twilight.Samples.CQRS.Data.Entities;

namespace Twilight.Samples.CQRS.Data;

public sealed class ViewDataContext : DbContext
{
    public ViewDataContext(DbContextOptions<ViewDataContext> options)
        : base(options)
    {
    }

    public DbSet<UserViewEntity> UsersView { get; set; } = null!;
}
