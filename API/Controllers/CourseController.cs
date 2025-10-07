using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Application.ServiceContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CourseController(ICourseService _courseService) : BaseApiController
{
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
            return Ok(await _courseService.GetAll());
        }

        var course = await _courseService.GetAll(userId.Value);
        return Ok(course);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<StudentCourseDetailsViewModel>>> StudentCourseDetails(long id)
    {
        var userId = User.GetUserId();
        var course = await _courseService.GetStudentCourseDetails(id, userId ?? 0);
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

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<long>>> GetcourseByModuleId(long id)
    {
        return Ok(await _courseService.GetCourseIdbyModule(id));
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

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<CourseStatisticsResponse>>> GetStatistics(long id)
    {
        return Ok(await _courseService.GetCourseStatistics(id));
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<bool>>> EnrollCourse(long id)
    {
        var userid = User.GetUserId();
        return Ok(await _courseService.EnrollCourse(id, userid ?? 0));
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<FilterResponseModel<MyLearningResponse>>>> MyCourses()
    {
        var userId = User.GetUserId();
        return Ok(await _courseService.MyCourses(userId ?? 0));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<FilterResponseModel<MyLearningResponse>>>>
        GetFilteredMyCourses(Filter filter)
    {
        var userId = User.GetUserId();
        return Ok(await _courseService.GetFilteredMyCourses(userId ?? 0, filter));
    }
}