using Microsoft.EntityFrameworkCore;
using Rotation.Infra.Activities;

namespace Rotation.Infra.Persistence;

internal class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ActivityMapping).Assembly);

        base.OnModelCreating(modelBuilder);
    }    
}
