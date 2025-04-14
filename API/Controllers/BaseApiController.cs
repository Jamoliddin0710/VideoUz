using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]/[action]")]
[ApiController]
[Authorize]
public class BaseApiController : ControllerBase
{
    protected new ActionResult<ServiceResponse<T>> Ok<T>(T obj)
    {
        return base.Ok(new ServiceResponse<T>()
        {
            Data = obj,
            IsSuccessful = true
        });
    }
    
    protected new ActionResult<ServiceResponse<object>> Ok()
    {
        return base.Ok(new ServiceResponse<object>()
        {
            IsSuccessful = true
        });
    }
}