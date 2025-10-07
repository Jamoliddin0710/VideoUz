using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class LikeDislike : BaseEntity
{   
    public long AppUserId { get; set; }
    public long VideoId { get; set; }

    public bool IsLike { get; set; } = true;
    //Navigation
    [ForeignKey(nameof(AppUserId))]
    public virtual AppUser AppUser { get; set; }
    [ForeignKey(nameof(VideoId))]
    public virtual Video Video { get; set; }
}