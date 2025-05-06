using Application.DTOs;
using Application.ServiceContract;
using Domain.Entities;
using Domain.RepositoryContracts;
using Mapster;

namespace Application.Services;

public class VideoService : IVideoService
{
    private readonly IUnitOfWork _unitOfWork;

    public VideoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Task Create(CreateOrEditVideoDTO model, long userId)
    {
       throw new NotImplementedException();
    }
}