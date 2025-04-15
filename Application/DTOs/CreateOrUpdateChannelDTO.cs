using System.ComponentModel.DataAnnotations;
using UI.Models;

namespace Application.DTOs;

public class CreateOrUpdateChannelDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public List<ErrorViewModel> Errors { get; set; }
}