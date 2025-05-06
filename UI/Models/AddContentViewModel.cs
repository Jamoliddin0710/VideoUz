using System.ComponentModel.DataAnnotations;

namespace UI.Models;

public class AddContentViewModel
{
    [Required]
    public int ModuleId { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string ContentType { get; set; }
    
    public IFormFile File { get; set; }
    public string TextContent { get; set; }
}