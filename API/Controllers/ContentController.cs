using Application.DTOs;
using Application.Models;
using Application.ServiceContract;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ContentController(IContentService contentService) : BaseApiController
{
    public async Task<ActionResult<ServiceResponse<bool>>> Create(CreateContentDTO contentDto)
    {
        return Ok(await contentService.Create(contentDto));
    }
}