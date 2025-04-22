using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Quiz : BaseEntity
{
    [Required]
    public long ContentId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    public string Description { get; set; }

    public int PassingScore { get; set; }

    public int TimeLimit { get; set; } // in minutes, 0 means no limit

    // Navigation properties
    [ForeignKey("ContentId")]
    public virtual Content Content { get; set; }

    public virtual ICollection<Question> Questions { get; set; }
    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; }
}