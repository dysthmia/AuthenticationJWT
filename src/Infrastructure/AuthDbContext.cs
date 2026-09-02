using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public sealed class AuthDbContext (DbContextOptions<DbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
    }
}
