using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class QuestionOption : BaseEntity
{
    [Required]
    public long QuestionId { get; set; }

    [Required]
    public string OptionText { get; set; }

    public bool IsCorrect { get; set; }
    
    [ForeignKey("QuestionId")]
    public virtual Question Question { get; set; }
}