using System.ComponentModel.DataAnnotations;
using UI.Models;

namespace Application.DTOs;

public class CreateOrUpdateChannelDTO
{
    public long? Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
}