namespace Application.Models;

public class CourseDetailViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string CoverImageUrl { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedDate { get; set; }
    public CategoryViewModel Category { get; set; }
    public List<ModuleViewModel> Modules { get; set; }
}

public class CategoryViewModel
{
    public long Id { get; set; }
    public string Name { get; set; }
}

public class ModuleViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public List<ContentViewModel> Contents { get; set; }
}

public class ContentViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string ContentType { get; set; }
    public int Order { get; set; }
    public int? Duration { get; set; } 
}