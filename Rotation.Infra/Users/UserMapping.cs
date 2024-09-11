using Microsoft.EntityFrameworkCore;
using Rotation.Application.Features.Users;

namespace Rotation.Infra.Users;

public class UserMapping
{
    public static void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(activity =>
        {
            activity.HasKey(x => x.Id);
            activity.Property(x => x.Id).ValueGeneratedOnAdd();
            
            activity.Property(x => x.Email).IsRequired();
        }); 
    }
}
