using Microsoft.EntityFrameworkCore;
using Twilight.Samples.Common.Data.Entities;

namespace Twilight.Samples.Common.Data;

public sealed class ViewDataContext(DbContextOptions<ViewDataContext> options) : DbContext(options)
{
    public DbSet<UserViewEntity> UsersView { get; init; } = null!;
}
