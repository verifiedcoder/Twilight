using Microsoft.EntityFrameworkCore;
using Twilight.Sample.Data.Entities;

namespace Twilight.Sample.Data
{
    public sealed class UsersViewContext : DbContext
    {
        public UsersViewContext(DbContextOptions<UsersViewContext> options)
            : base(options)
        {
        }

        public DbSet<UserViewEntity> UsersView { get; set; } = null!;
    }
}
