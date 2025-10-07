using Application.DTOs;

namespace Application.ServiceContract;

public interface IVideoService
{
    Task Create(CreateOrEditVideoDTO model, long userId);
}