using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class RegisterDTO
{
    [Required (ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    [Required (ErrorMessage = "Username is required")]
    [StringLength(maximumLength: 15,  ErrorMessage = "Username length must be between 3 and 15 characters")]
    [MinLength(3, ErrorMessage = "Username length must be between  3 and 15 characters")]
    [Display(Name = "Username")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "Confirm Password is required")]
    public string ConfirmPassword { get; set; }
}