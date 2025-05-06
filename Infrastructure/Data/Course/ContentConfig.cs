using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Course
{
    public class ContentConfig : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.HasOne(c => c.Module)
                .WithMany(m => m.Contents)
                .HasForeignKey(c => c.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
