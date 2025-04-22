using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class QuizAnswer : BaseEntity
{
    [Required]
    public long AttemptId { get; set; }

    [Required]
    public long QuestionId { get; set; }

    public string AnswerText { get; set; }

    public long? SelectedOptionId { get; set; }

    public bool IsCorrect { get; set; }
    
    [ForeignKey("AttemptId")]
    public virtual QuizAttempt Attempt { get; set; }

    [ForeignKey("QuestionId")]
    public virtual Question Question { get; set; }

    [ForeignKey("SelectedOptionId")]
    public virtual QuestionOption SelectedOption { get; set; }
}