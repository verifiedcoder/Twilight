using Microsoft.EntityFrameworkCore;
using Twilight.Sample.Data.Entities;

namespace Twilight.Sample.Data
{
    public sealed class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; } = null!;
    }
}
