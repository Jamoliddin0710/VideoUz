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

    public string Description { get; set; } = string.Empty;

    public int PassingScore { get; set; } = 60;

    public int TimeLimit { get; set; } 
    
    [ForeignKey("ContentId")]
    public virtual Content Content { get; set; }

    public virtual ICollection<Question> Questions { get; set; }
    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; }
}