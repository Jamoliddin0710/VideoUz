using Application.DTOs;
using Application.Models;

namespace Application.ServiceContract;

public interface IContentService
{
    Task<bool> Create(CreateContentDTO createViewModel);
}