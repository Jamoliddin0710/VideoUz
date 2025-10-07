using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Category : BaseEntity
{
    [Required]
    public string Name { get; set; }

    public string Description { get; set; }
    public bool IsActive { get; set; }
    public virtual ICollection<Video> Videos { get; set; }
    public virtual ICollection<Course> Courses { get; set; }
}