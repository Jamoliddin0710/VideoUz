using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Module : BaseEntity
{
    [Required]
    public long CourseId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    public int Order { get; set; }

    [ForeignKey("CourseId")]
    public virtual Course Course { get; set; }

    public virtual ICollection<Content> Contents { get; set; }
}