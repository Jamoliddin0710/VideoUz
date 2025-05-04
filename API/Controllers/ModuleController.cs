using Application.DTOs;
using Application.ServiceContract;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ModuleController : BaseApiController
{
    private readonly IModuleService _moduleService;

    public ModuleController(IModuleService moduleService)
    {
        _moduleService = moduleService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<FilterResponseModel<ModuleDTO>>>> GetModulesByCourses(long courseId)
    {
        var courses = await _moduleService.GetModuleByCourseId(courseId);
        return Ok(courses);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<ModuleDTO>>> Create([FromBody] CreateModuleDTO dto)
    {
        var result = await _moduleService.Create(dto);
        return Ok(result);
    }
}