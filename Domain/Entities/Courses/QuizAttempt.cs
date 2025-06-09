using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class QuizAttempt : BaseEntity
{
    [Required]
    public long QuizId { get; set; }

    [Required]
    public long UserId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int Score { get; set; }
    public int MaxScore { get; set; }
    public int CorrectAnswerCount { get; set; }

    public bool IsPassed { get; set; }
    
    [ForeignKey("QuizId")]
    public virtual Quiz Quiz { get; set; }

    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; }

    public virtual ICollection<QuizAnswer> Answers { get; set; }
}