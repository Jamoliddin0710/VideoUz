using Application.DTOs;
using Application.Models;

namespace Application.ServiceContract;

public interface IContentService
{
    Task<ContentDTO> Create(CreateContentDTO createViewModel);
    Task<ContentDTO> GetById(long id);
    Task<bool> Delete(long id);
}