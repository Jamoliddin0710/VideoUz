namespace Application.Models;

public class VideoContentViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ContentData { get; set; } // Video URL
    public string ContentType { get; set; }
    public int? Duration { get; set; }
    public int Order { get; set; }
    
    public ModuleViewModel Module { get; set; }
    public CourseViewModel Course { get; set; }
    public EnrollmentViewModel EnrolledUser { get; set; }
}

public class CourseViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string AuthorId { get; set; }
    public CategoryViewModel Category { get; set; }
    public List<ModuleViewModel> Modules { get; set; }
}

public class EnrollmentViewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrolledAt { get; set; }
    public string Status { get; set; }
    public List<ProgressViewModel> Progress { get; set; }
}

public class ProgressViewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public string Status { get; set; }
    public DateTime? CompletedDate { get; set; }
}