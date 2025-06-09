namespace Application.Models;


public class VideoContentViewModelV1
{
    public ContentDto Content { get; set; }

    public ModuleDto Module { get; set; }

    public CourseDto Course { get; set; }
  
    public List<ModuleDto> AllModules { get; set; }
    
    public int ModuleContentsCount { get; set; }
    public List<ProgressDto> ContentProgress { get; set; }
}

public class ContentDto
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public string Title { get; set; }
    public string ContentType { get; set; }
    public string ContentData { get; set; }
    public int? Duration { get; set; }
    public int Order { get; set; }
}

public class ModuleDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public List<ContentDto> Contents { get; set; }
}

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}

public class ProgressDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ContentId { get; set; }
    public string Status { get; set; }
    public DateTime? CompletedDate { get; set; }
}