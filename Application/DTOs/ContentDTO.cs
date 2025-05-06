using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs;

public class ContentDTO
{
    public long ModuleId { get; set; }
    [Required]
    [StringLength(200)]
    public string Title { get; set; }
    [Required]
    public ContentType ContentType { get; set; } 
    public string ContentData { get; set; }
    public int Order { get; set; }
    public int? Duration { get; set; } 
}