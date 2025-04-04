using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Category : BaseEntity
{
    [Required]
    public string Name { get; set; }
    public virtual ICollection<Video> Videos { get; set; }
}