using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.DTOs;

public class UserDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email  { get; set; }
    public string Role { get; set; }
    public string Phone { get; set; }
    public List<string> AvailableRoles { get; set; }
}

public class UpdateUserDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Role { get; set; }
}