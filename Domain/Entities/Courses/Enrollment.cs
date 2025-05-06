using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Enrollment : BaseEntity
{
    [Required]
    public long UserId { get; set; }

    [Required]
    public long CourseId { get; set; }

    public DateTime EnrollDate { get; set; }

    [Required]
    public EnrollmentStatus Status { get; set; } 
    
    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; }

    [ForeignKey("CourseId")]
    public virtual Course Course { get; set; }
}