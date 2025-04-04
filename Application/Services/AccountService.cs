using Application.ServiceContract;
using Domain.Entities;
using Infrastructure.DTOs;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;

    public AccountService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AppUserResponse> FindByUsername(string username)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == username);
        return user.Adapt<AppUserResponse>();
    }
}