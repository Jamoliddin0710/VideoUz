using Application.DTOs;
using Application.ServiceContract;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ModuleController(IModuleService _moduleService) : BaseApiController
{
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

    [HttpDelete]
    public async Task<ActionResult<object>> Delete(long moduleId)
    {
        var result = await _moduleService.Delete(moduleId);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<ModuleDTO>>> GetById(long Id)
    {
        return Ok(await _moduleService.GetById(Id));
    }
}