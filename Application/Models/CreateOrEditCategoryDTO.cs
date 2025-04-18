using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class CreateOrEditCategoryDTO
{
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
}