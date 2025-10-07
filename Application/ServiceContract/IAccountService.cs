using Application.DTOs;
using Infrastructure.DTOs;

namespace Application.ServiceContract;

public interface IAccountService
{
    Task<AppUserResponse> FindByUsername(string username);
    Task<FilterResponseModel<UserDTO>> GetUsers(); 
    Task<bool> Update(UpdateUserDTO userDto); 
    Task<bool> Delete(long id); 
    Task<FilterResponseModel<string>> GetAllRoles(); 
}