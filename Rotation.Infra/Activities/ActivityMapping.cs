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
        }); 
    }
}
