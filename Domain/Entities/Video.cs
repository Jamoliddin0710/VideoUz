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
    public Category Category { get; set; }
    [ForeignKey(nameof(ChannelId))]
    public Channel Channel { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<LikeDislike> LikeDislikes { get; set; }
}