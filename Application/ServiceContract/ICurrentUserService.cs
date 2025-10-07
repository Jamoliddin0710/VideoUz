namespace Application.ServiceContract;

public interface ICurrentUserService
{
    string GetUserId();
    string GetIpAddress();
}