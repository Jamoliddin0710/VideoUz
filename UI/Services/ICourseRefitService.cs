using Application.DTOs;
using Application.Helpers;
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
    [Get("/course/StudentCourseDetails")]
    Task<ServiceResponse<StudentCourseDetailsViewModel>> StudentCourseDetails(long id);
    [Get("/course/getall")]
    Task<ServiceResponse<FilterResponseModel<CourseListViewModel>>> GetAll();
    
    [Get("/course/details")]
    Task<ServiceResponse<CourseDetailViewModel>> GetCourseWithDetails(long courseId);
    [Delete("/course/delete")]
    Task<ServiceResponse<CourseDetailViewModel>> Delete(long id);
    [Get("/course/GetcourseByModuleId")]
    Task<ServiceResponse<long?>> GetcourseByModuleId(long id);   
    [Get("/course/getcoursebymodule")]
    Task<ServiceResponse<CourseDTO?>> GetcourseByModule(long Id);  
    [Get("/course/GetCourseIdbyModule")]
    Task<ServiceResponse<CourseDTO?>> GetCourseIdbyModule(long moduleId);  
    [Get("/course/published")]
    Task<ServiceResponse<object?>> PublishCourse(long Id);
    [Get("/course/enrollcourse")]
    Task<ServiceResponse<object?>> EnrolleCourse(long Id);
    [Get("/course/getstatistics")]
    Task<ServiceResponse<CourseStatisticsResponse?>> GetStatistics(long id);
    [Get("/course/mycourses")]
    Task<ServiceResponse<FilterResponseModel<MyLearningResponse>>> GetMyCourses();
    [Post("/course/getfilteredmycourses")]
    Task<ServiceResponse<FilterResponseModel<MyLearningResponse>>> GetFilteredMyCourses([Body] Filter filter);
}