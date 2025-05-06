using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace UI.Services;

public interface IContentRefitService
{
    [Post("/content/create")]
    Task<ActionResult<ServiceResponse<bool>>> Create(CreateContentDTO dto);
}