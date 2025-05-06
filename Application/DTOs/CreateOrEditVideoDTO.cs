using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.DTOs;

public class CreateOrEditVideoDTO
{
    public long Id { get; set; }
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    [Required]
    [Display(Name = "Please upload image for video")]
    public IFormFile ImageFile { get; set; }
    [Required]
    [Display(Name = "Please upload video file")]
    public IFormFile VideoFile { get; set; }
    [Required]
    [Display(Name = "Please select category for video")]
    public long CategoryId { get; set; }
    public string ImageContentTypes { get; set; }
    public string VideoContentTypes { get; set; }
    public string ImageUrl { get; set; }
    public List<SelectListItem> CategoryDropDown { get; set; }
}