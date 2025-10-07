using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Progress : BaseEntity
{
    [Required]
    public long UserId { get; set; }

    [Required]
    public long ContentId { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } 

    public DateTime? CompletedDate { get; set; }
    
    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; }

    [ForeignKey("ContentId")]
    public virtual Content Content { get; set; }
}