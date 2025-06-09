using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Application.Models;
using Infrastructure.DTOs;
using Infrastructure.ViewModels;
using Refit;

namespace UI.Services;

public interface IAccountRefitClient
{
    [Post("/account/login")]
    Task<ServiceResponse<TokenModel>> Login([Body]  LoginDTO loginDto);  
    [Post("/account/register")]
    Task<ServiceResponse<TokenModel>> Register([Body]  RegisterDTO registerDto);
    [Get("/account/getallusers")]
    Task<ServiceResponse<FilterResponseModel<UserDTO>>> GetAllUsers();
    [Put("/account/update")]
    Task<ServiceResponse<bool>> Update(UpdateUserDTO model);
    [Delete("/account/delete")]
    Task<ServiceResponse<bool>> Delete(long id);
    [Get("/account/getallroles")]
    Task<ServiceResponse<FilterResponseModel<string>>> GetAllRoles();
}