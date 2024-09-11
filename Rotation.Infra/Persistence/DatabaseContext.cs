using Microsoft.EntityFrameworkCore;
using Rotation.Infra.Activities;
using Rotation.Infra.Users;

namespace Rotation.Infra.Persistence;

internal class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ActivityMapping.Map(modelBuilder);
        UserMapping.Map(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }    
}
