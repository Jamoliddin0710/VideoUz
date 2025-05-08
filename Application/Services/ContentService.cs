using Application.DTOs;
using Application.Models;
using Application.ServiceContract;
using Domain.Entities;
using Domain.RepositoryContracts;
using Mapster;

namespace Application.Services;

public class ContentService(IUnitOfWork _unitOfWork) : IContentService
{
    public async Task<ContentDTO> Create(CreateContentDTO model)
    {
        var content = model.Adapt<Content>();
        _unitOfWork.ContentRepo.Add(content);
        await _unitOfWork.CompleteAsync();
        return content.Adapt<ContentDTO>();
    }

    public async Task<ContentDTO> GetById(long id)
    {
        var content = await _unitOfWork.ContentRepo.GetByIdAsync(id);
        return content.Adapt<ContentDTO>();
    }

    public async Task<bool> Delete(long id)
    {
        var content = await _unitOfWork.ContentRepo.GetByIdAsync(id);
        _unitOfWork.ContentRepo.Remove(content);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}