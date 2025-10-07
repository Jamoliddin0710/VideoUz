using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Video : BaseEntity
{
    public string ThumbnailUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ContentType { get; set; }
    public byte[] Contents { get; set; }
    public long CategoryId { get; set; }
    public long ChannelId { get; set; }
    
    //Navigation
    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; }
    [ForeignKey(nameof(ChannelId))]
    public virtual Channel Channel { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<LikeDislike> LikeDislikes { get; set; }
}