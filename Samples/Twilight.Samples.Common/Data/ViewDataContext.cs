using Microsoft.EntityFrameworkCore;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Data;

public sealed class ViewDataContext : DbContext
{
    public ViewDataContext(DbContextOptions<ViewDataContext> options)
        : base(options)
    {
    }

    public DbSet<UserViewEntity> UsersView { get; set; } = null!;
}
