using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CreateOrUpdateChannelDTO
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
}