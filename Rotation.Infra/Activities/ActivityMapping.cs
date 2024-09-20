using Microsoft.EntityFrameworkCore;
using Rotation.Application.Features.Activities;

namespace Rotation.Infra.Activities;

public class ActivityMapping
{
    public static void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(activity =>
        {
            activity.HasKey(x => x.Id);

            activity.Property(x => x.Id).ValueGeneratedOnAdd();

            activity.Property(x => x.Duration).HasColumnType("jsonb");
            activity.Property(x => x.Resume).IsRequired(false).HasColumnType("jsonb");
        });

        modelBuilder.Entity<Activity>()
            .HasMany(m => m.Users)
            .WithMany(m => m.Activities);
    }
}