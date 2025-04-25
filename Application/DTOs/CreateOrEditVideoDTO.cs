using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.DTOs;

public class CreateOrEditVideoDTO
{
    public long Id { get; set; }
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    [Required]
    [Display(Name = "Paease upload image for vided")]
    public FormFile ImageFile { get; set; }
    [Required]
    [Display(Name = "Please upload video file")]
    public FormFile VideoFile { get; set; }
    [Required]
    [Display(Name = "Please select category for video")]
    public long CategoryId { get; set; }
    public string ImageContentType { get; set; }
    public string VideoContentType { get; set; }
    public string ImageUrl { get; set; }
    public List<SelectListItem> CategoryDropDown { get; set; }
}