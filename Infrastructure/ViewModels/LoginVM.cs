using System.ComponentModel.DataAnnotations;

namespace Infrastructure.ViewModels;

public class LoginVM
{
    public string? ReturnUrl { get; set; }
    [Required(ErrorMessage = "Username is required")]
    [Display(Name = "Put your Username or Email")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}