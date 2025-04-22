using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Domain.Entities;

public class Course : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [StringLength(2000)]
    public string Description { get; set; }

    [Required]
    public long AuthorId { get; set; }

    [Required]
    public long CategoryId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public string CoverImagePath { get; set; }

    public bool IsPublished { get; set; } = false;
    
    [ForeignKey("AuthorId")]
    public virtual AppUser Author { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }

    public virtual ICollection<Module> Modules { get; set; }
    public virtual ICollection<Enrollment> Enrollments { get; set; }
}