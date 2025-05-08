using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Application.ServiceContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CourseController : BaseApiController
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<CourseDTO>>> Create([FromBody] CourseCreateDTO courseCreateDto)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var result = await _courseService.Create(courseCreateDto, userId.Value);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<CourseDTO>>> GetById(long courseId)
    {
        var course = await _courseService.GetbyId(courseId);
        return Ok(course);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<FilterResponseModel<CourseListViewModel>>>> GetAll()
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var course = await _courseService.GetAll(userId.Value);
        return Ok(course);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<CourseDetailViewModel>>> Details(long courseId)
    {
        return Ok(await _courseService.GetCourseDetails(courseId));
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<CourseDTO>>> GetcourseByModule(long modelId)
    {
        return Ok(await _courseService.GetcourseByModule(modelId));
    }

    [HttpDelete]
    public async Task<ActionResult<ServiceResponse<bool>>> Delete(long id)
    {
        return Ok(await _courseService.Delete(id));
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<bool>>> Published(long id)
    {
        return Ok(await _courseService.PublishCourse(id));
    }
}