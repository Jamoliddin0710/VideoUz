using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Content : BaseEntity
{
    [Required]
    public long ModuleId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(50)]
    public string ContentType { get; set; } // "Video", "Text", "Document", "Quiz"

    public string ContentData { get; set; }

    public int Order { get; set; }

    // Additional properties for specific content types
    public int? Duration { get; set; } // For videos (in seconds)

    // Navigation properties
    [ForeignKey("ModuleId")]
    public virtual Module Module { get; set; }

    public virtual ICollection<Progress> Progresses { get; set; }
    public virtual ICollection<Quiz> Quizzes { get; set; }
}