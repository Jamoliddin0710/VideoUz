using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Subscribe : BaseEntity
{
    public long ChannelId { get; set; }
    public long AppUserId { get; set; }
    //Navigation
    [ForeignKey(nameof(AppUserId))]
    public AppUser AppUser { get; set; }
    [ForeignKey(nameof(ChannelId))]
    public Channel Channel { get; set; }
}