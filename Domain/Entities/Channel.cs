using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Channel : BaseEntity
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public long AppUserId { get; set; }
    //Navigation
    [ForeignKey(nameof(AppUserId))]
    public AppUser AppUser { get; set; }
    
    public virtual ICollection<Video> Videos { get; set; }
    public virtual ICollection<Subscribe> Subscribers{ get; set; }
}