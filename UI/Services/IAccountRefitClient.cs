using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Application.Models;
using Infrastructure.DTOs;
using Infrastructure.ViewModels;
using Refit;

namespace Application.ServiceContract;

public interface IAccountRefitClient
{
    [Post("/account/login")]
    Task<ServiceResponse<TokenModel>> Login([Body]  LoginDTO loginDto);  
    [Post("/account/register")]
    Task<ServiceResponse<TokenModel>> Register([Body]  RegisterDTO registerDto);
}