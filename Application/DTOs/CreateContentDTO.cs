using Domain.Entities;

namespace Application.DTOs;

public class CreateContentDTO
{
    public long ModuleId { get; set; }
    public string Title { get; set; }
    public ContentType ContentType { get; set; }
    public string? ContentData { get; set; }
    public long? FileId { get; set; }
}