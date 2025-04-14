using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class LikeDislikeConfig : IEntityTypeConfiguration<LikeDislike>
{
    public void Configure(EntityTypeBuilder<LikeDislike> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.AppUser).WithMany(a => a.LikeDislikes).HasForeignKey(a => a.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a => a.Video).WithMany(a => a.LikeDislikes).HasForeignKey(a => a.VideoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}