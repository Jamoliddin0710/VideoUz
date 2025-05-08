using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace UI.Services;

public interface IContentRefitService
{
    [Post("/content/create")]
    Task<ServiceResponse<ContentDTO>> Create(CreateContentDTO dto);
    [Get("/content/getbyid")]
    Task<ServiceResponse<ContentDTO>> GetById(long id);
    [Delete("/content/delete")]
    Task<ServiceResponse<object>> Delete(long id);
}