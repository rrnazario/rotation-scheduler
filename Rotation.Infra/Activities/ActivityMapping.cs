using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Activity = Rotation.Application.Features.Activities.Activity;

namespace Rotation.Infra.Activities;

public class ActivityMapping 
    : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Duration).HasColumnType("jsonb");
        builder.Property(x => x.Resume).IsRequired(false).HasColumnType("jsonb");

        builder.HasMany(m => m.Users).WithMany(m => m.Activities);
    }
}