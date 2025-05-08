using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class CourseConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(a=>a.Id);
        builder.HasOne(a => a.Category).WithMany(a=>a.Courses).HasForeignKey(a => a.CategoryId);
        builder.HasOne(a => a.Author).WithMany(a=>a.AuthoredCourses).HasForeignKey(a => a.AuthorId);
        builder.HasMany(a => a.Modules).WithOne(a => a.Course).HasForeignKey(a => a.CourseId);
    }
}