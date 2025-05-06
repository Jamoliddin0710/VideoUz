using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Course;

public class QuizConfig : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasOne(q => q.Content)
            .WithMany(c => c.Quizzes)
            .HasForeignKey(q => q.ContentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}