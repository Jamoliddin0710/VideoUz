namespace Application.DTOs;

public class CourseListViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string CoverImageUrl { get; set; }
    public string Category { get; set; }
    public bool IsPublished { get; set; }
    public int ModulesCount { get; set; }
    public int StudentsCount { get; set; }
    public string User { get; set; }
    public DateTime CreatedDate { get; set; }
}