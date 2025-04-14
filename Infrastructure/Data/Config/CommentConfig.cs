using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(a=>a.Id);
        builder.HasOne(a=>a.Video).WithMany(a=>a.Comments).HasForeignKey(a=>a.VideoId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a => a.AppUser).WithMany(a => a.Comments).HasForeignKey(a => a.AppUserId).OnDelete(DeleteBehavior.Restrict);
    }
}