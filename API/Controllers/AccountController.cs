using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs;
using Application.Models;
using Application.Utility;
using Domain.Entities;
using Infrastructure.DTOs;
using Infrastructure.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ServiceResponse<TokenModel>>> Login([FromBody]LoginDTO loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(loginDto.UserName);
            
            if (user == null)
            {
                return Unauthorized(new ServiceResponse<TokenModel>
                {
                    IsSuccessful = false,
                    Error = new ErrorModel("401", "User not found")
                });

            }
        }
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (result.Succeeded)
        {
            var token = await GenerateJwtToken(user);
            return new ServiceResponse<TokenModel>()
            {
                Data = token,
                IsSuccessful = true,
            };
        }
        
        if (result.IsLockedOut)
        {
            return BadRequest(new ServiceResponse<TokenModel>
            {
                IsSuccessful = false,
                Error = new ErrorModel("403", "your account locked out please try again later")
            });
        }

        return new ServiceResponse<TokenModel>
        {
            IsSuccessful = false,
            Error = new ErrorModel("400", "Invalid login or password")
        };
    }

    private async Task<bool> CheckUserNameExists(string userName)
    {
        return _userManager.Users.Any(u => u.UserName == userName);
    }
    
    private async Task<bool> CheckEmailExists(string email)
    {
        return _userManager.Users.Any(u => u.UserName == email);
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ServiceResponse<TokenModel>>> Register(RegisterDTO userRegisterDto)
    {
        if (await CheckUserNameExists(userRegisterDto.Name))
        {
            throw new ApplicationException("User already exists Username is dublicate");
        }
      
        if (await CheckEmailExists(userRegisterDto.Email))
        {
            throw new ApplicationException("User already exists Email is dublicate");
        }

        var user = userRegisterDto.Adapt<AppUser>();
        user.UserName = userRegisterDto.Name;
        var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
        
        if (!result.Succeeded)
        {
            return new ServiceResponse<TokenModel>()
            {
                Error = new ErrorModel()
                {
                    Details = result.Errors.Select(a => a.Description).ToList(),
                }
            };
        }
        
        await _userManager.AddToRoleAsync(user, nameof(Role.User));
        
        var token = await GenerateJwtToken(user);
        return new ServiceResponse<TokenModel>()
        {
            Data = token,
            IsSuccessful = true,
        };
    }

    private async Task<TokenModel> GenerateJwtToken(AppUser user)
    {
        // Create claims identity with default authentication scheme
        var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

        // Add claims
        claimIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
        claimIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        claimIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
        claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        // Add issued and expiration claims
        claimIdentity.AddClaim(new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddHours(24).ToString())); 
        var roles = _userManager.GetRolesAsync(user).Result;
        foreach (var role in roles)
        {
            claimIdentity.AddClaim(new Claim(ClaimTypes.Role, role));  
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("testusername0012232434343863486837467834"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create token
        var token = new JwtSecurityToken(
            issuer: "localhost:5151",  
            audience: "localhost:5261",
            claims: claimIdentity.Claims,
            expires: (DateTime.UtcNow.AddHours(24)),
            signingCredentials: creds
        );
        
        var tokenModel = new TokenModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),  
            Expiration = token.ValidTo,
            RefreshToken = Guid.NewGuid().ToString(), 
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(5),  
        };

        return tokenModel;
    }

}