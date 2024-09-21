using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rotation.Application.Features.Users;

namespace Rotation.Infra.Users;

public class UserMapping
    : IEntityTypeConfiguration<User>
{   
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Email).IsRequired();
    }
}
