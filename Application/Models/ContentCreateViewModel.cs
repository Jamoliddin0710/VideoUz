using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Models;

public class ContentCreateViewModel
{
    public long ModuleId { get; set; }

    [Required(ErrorMessage = "Kontent nomi majburiy")]
    [StringLength(200, ErrorMessage = "Kontent nomi 200 belgidan oshmasligi kerak")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Kontent turi majburiy")]
    public ContentType ContentType { get; set; } = ContentType.Video;

    public string? ContentData { get; set; }

    public IFormFile? File { get; set; }
}

public class ContentEditViewModel
{
    public long Id { get; set; }
    public long ModuleId { get; set; }

    [Required(ErrorMessage = "Kontent nomi majburiy")]
    [StringLength(200, ErrorMessage = "Kontent nomi 200 belgidan oshmasligi kerak")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Kontent turi majburiy")]
    public ContentType ContentType { get; set; }

    public string? ContentData { get; set; }

    [Display(Name = "Fayl")] public IFormFile? File { get; set; }
    public string FileName { get; set; }
    public string Bucket { get; set; }
}