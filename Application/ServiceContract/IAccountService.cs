using Infrastructure.DTOs;

namespace Application.ServiceContract;

public interface IAccountService
{
    Task<AppUserResponse> FindByUsername(string username);
}