using System.ComponentModel.DataAnnotations;

namespace UI.Models;

public class ModuleCreateViewModel
{
    public long CourseId { get; set; }
    
    [Required(ErrorMessage = "Module title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    [Display(Name = "Module Title")]
    public string Title { get; set; }
    
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    [Display(Name = "Module Description")]
    public string Description { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Order must be greater than 0")]
    [Display(Name = "Module Order")]
    public int Order { get; set; }
}