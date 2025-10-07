using System.ComponentModel.DataAnnotations;

namespace UI.Models;

public class CourseCreateViewModel
{
    [Required]
    [Display(Name = "Course Title is required")]
    public string Title { get; set; }

    [Required]
    [Display(Name = "Course Description is required")]
    public string Description { get; set; }

    [Required]
    [Display(Name = "You must select category")]
    public long CategoryId { get; set; }

    [Required] public decimal Price { get; set; }

    [Required]
    [Display(Name = "You must upload image")]
    public IFormFile CoverImage { get; set; }
}

public class CourseEditViewModel
{
    public long Id { get; set; }

    [Required]
    [Display(Name = "Course Title is required")]
    public string Title { get; set; }

    [Required]
    [Display(Name = "Course Description is required")]
    public string Description { get; set; }

    [Required]
    [Display(Name = "You must select category")]
    public long CategoryId { get; set; }

    [Required] public decimal Price { get; set; }

    [Required]
    [Display(Name = "You must upload image")]
    public IFormFile CoverImage { get; set; }
}