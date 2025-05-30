using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Comment : BaseEntity
{
    [Required]
    public string Content { get; set; }
    public long AppUserId { get; set; }
    public long VideoId { get; set; }
    //Navigation
    [ForeignKey(nameof(AppUserId))]
    public AppUser AppUser { get; set; }
    [ForeignKey(nameof(VideoId))]
    public Video Video { get; set; }
}