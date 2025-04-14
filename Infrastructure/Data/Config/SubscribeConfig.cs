using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class SubscribeConfig : IEntityTypeConfiguration<Subscribe>
{
    public void Configure(EntityTypeBuilder<Subscribe> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(a => a.AppUser).WithMany(a=>a.Subscribtions).HasForeignKey(a=>a.AppUserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a=>a.Channel).WithMany(a=>a.Subscribers).HasForeignKey(a=>a.ChannelId).OnDelete(DeleteBehavior.Restrict);
    }
}