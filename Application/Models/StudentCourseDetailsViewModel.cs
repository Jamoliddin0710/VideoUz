namespace Application.Models;

public class StudentCourseDetailsViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string CoverImageUrl { get; set; }
    public CategoryViewModel Category { get; set; }
    public bool IsEnrolled { get; set; }
    public CourseProgressDTO Progress { get; set; }
    public List<StudentModuleViewModel> Modules { get; set; }
}

public class CourseProgressDTO
{
    public int CompletionPercentage { get; set; }
}

public class StudentModuleViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public bool IsCompleted { get; set; }
    public List<StudentContentViewModel> Contents { get; set; }
}

public class StudentContentViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string ContentType { get; set; }
    public int Order { get; set; }
    public int? Duration { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsLocked { get; set; }
}
