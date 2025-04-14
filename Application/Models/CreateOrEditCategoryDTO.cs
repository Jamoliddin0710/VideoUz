using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class CreateOrEditCategoryDTO
{
    [Required]
    public string Name { get; set; }
}