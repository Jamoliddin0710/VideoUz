using Application.DTOs;
using Application.Models;
using Domain.Entities;
using Refit;

namespace UI.Services;

public interface ICourseRefitService
{
    [Post("/course/create")]
    Task<ServiceResponse<CourseDTO>> Create([Body] CourseCreateDTO courseCreateDto);

    [Get("/course/getbyId")]
    Task<ServiceResponse<CourseDTO>> GetById(long courseId);
    [Get("/course/getall")]
    Task<ServiceResponse<FilterResponseModel<CourseListViewModel>>> GetAll();
    
    [Get("/course/details")]
    Task<ServiceResponse<CourseDetailViewModel>> GetCourseWithDetails(long courseId);
    [Delete("/course/delete")]
    Task<ServiceResponse<CourseDetailViewModel>> Delete(long id);
    [Get("/course/getcoursebymodule")]
    Task<ServiceResponse<CourseDTO?>> GetcourseByModule(long Id);   
    [Get("/course/published")]
    Task<ServiceResponse<object?>> PublishCourse(long Id);
}