using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs;

public class ContentDTO
{
    public long Id { get; set; }
    public long ModuleId { get; set; }
    public string Title { get; set; }
    public ContentType ContentType { get; set; }
    public string ContentData { get; set; }
    public int Order { get; set; }
    public int? Duration { get; set; }
    public FileItemDTO FileItem { get; set; }
}