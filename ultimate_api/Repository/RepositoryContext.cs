using Microsoft.EntityFrameworkCore;
using Entities.Models;

namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Company>? Companies { get; set; }
    }
}
