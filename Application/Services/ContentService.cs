using Application.DTOs;
using Application.Models;
using Application.ServiceContract;
using Domain.Entities;
using Domain.RepositoryContracts;
using Mapster;

namespace Application.Services;

public class ContentService(IUnitOfWork _unitOfWork) : IContentService
{
    public async Task<bool> Create(CreateContentDTO model)
    {
        var content = model.Adapt<Content>();
        _unitOfWork.ContentRepo.Add(content);
        return await _unitOfWork.CompleteAsync();
    }
}