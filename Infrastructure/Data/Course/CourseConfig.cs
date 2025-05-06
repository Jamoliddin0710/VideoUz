using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Data.Course
{
    public class CourseConfig : IEntityTypeConfiguration<Domain.Entities.Course>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Course> builder)
        {
            builder.HasOne(c => c.Author)
                .WithMany(u => u.AuthoredCourses)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
