using Application.DTOs;
using Application.ServiceContract;
using Domain.Entities;
using Infrastructure.DTOs;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AccountService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : IAccountService
{
    public async Task<AppUserResponse> FindByUsername(string username)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.UserName == username);
        return user.Adapt<AppUserResponse>();
    }

    public async Task<FilterResponseModel<UserDTO>> GetUsers()
    {
        var users = userManager.Users.ToList();

        var userDtos = new List<UserDTO>();

        foreach (var user in users)
        {
            var roles = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var roleList = roleManager.Roles.Select(a => a.Name).ToList();
            userDtos.Add(new UserDTO
            {
                Id = user.Id,
                Email = user?.Email ?? string.Empty,
                Name = user.Name,
                UserName = user.UserName ?? string.Empty,
                Phone = user?.PhoneNumber ?? string.Empty,
                Role = string.Join(", ", roles),
                AvailableRoles = roleList,
            });
        }

        return new FilterResponseModel<UserDTO>()
        {
            ItemsCount = userDtos.Count,
            Data = userDtos
        };
    }
    
    public async Task<bool> Update(UpdateUserDTO userDto)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.Id == userDto.Id);
        if (user == null)
            return false; 
        
        user.Email = userDto.Email;
        user.Name = userDto.Name;
        user.UserName = userDto.UserName;
        user.PhoneNumber = userDto.Phone;

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return false;

      
        if (!string.IsNullOrWhiteSpace(userDto.Role) &&
            await roleManager.RoleExistsAsync(userDto.Role))
        {
            var currentRoles = await userManager.GetRolesAsync(user);

            foreach (var role in currentRoles)
            {
                await userManager.RemoveFromRoleAsync(user, role);
            }

            await userManager.AddToRoleAsync(user, userDto.Role);
        }

        return true;
    }


    public async Task<bool> Delete(long id)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
        await userManager.DeleteAsync(user);
        return true;
    }

    public async Task<FilterResponseModel<string>> GetAllRoles()
    {
        var roles = roleManager.Roles.Select(a => a.Name).ToList();
        return new FilterResponseModel<string>()
        {
            ItemsCount = roles.Count,
            Data = roles
        };
    }
}