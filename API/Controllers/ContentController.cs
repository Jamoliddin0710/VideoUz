using Application.DTOs;
using Application.Models;
using Application.ServiceContract;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ContentController(IContentService contentService) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<ContentDTO>>> Create(CreateContentDTO contentDto)
    {
        return Ok(await contentService.Create(contentDto));
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<ContentDTO>>> GetById(int id)
    {
        return Ok(await contentService.GetById(id));
    }

    [HttpDelete]
    public async Task<ActionResult<object>> Delete(long id)
    {
        return Ok(await contentService.Delete(id));
    }
}