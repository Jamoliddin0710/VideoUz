using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Question : BaseEntity
{
    [Required]
    public long QuizId { get; set; }

    [Required]
    public string QuestionText { get; set; }

    [Required]
    [StringLength(50)]
    public string QuestionType { get; set; } 

    public int Points { get; set; }

    public int Order { get; set; }
    
    [ForeignKey("QuizId")]
    public virtual Quiz Quiz { get; set; }

    public virtual ICollection<QuestionOption> Options { get; set; }
}