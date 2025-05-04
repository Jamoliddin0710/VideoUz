using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser :  IdentityUser<long> 
{
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public virtual Channel Channel { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Subscribe> Subscribtions { get; set; }
    public virtual ICollection<LikeDislike> LikeDislikes { get; set; }
    public virtual ICollection<Course> AuthoredCourses { get; set; }
    public virtual ICollection<Enrollment> Enrollments { get; set; }
    public virtual ICollection<Progress> Progresses { get; set; }
}