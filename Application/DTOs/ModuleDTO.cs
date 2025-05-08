using Domain.Entities;

namespace Application.DTOs;

public class ModuleDTO
{
    public long Id { get; set; }
    public long CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public List<ContentDTO> Contents { get; set; }
    public CourseDTO? Course { get; set; }
}
public class CreateModuleDTO
{
    public long CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
}

