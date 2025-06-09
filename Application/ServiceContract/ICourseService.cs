using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Domain.Entities;

namespace Application.ServiceContract;

public interface ICourseService
{
    Task<CourseDTO> Create(CourseCreateDTO course, long userId);
    Task Update(Course course);
    Task<CourseDTO> GetbyId(long courseId);
    Task<CourseDTO> GetcourseByModule(long moduleId);
    Task<FilterResponseModel<CourseListViewModel>> GetAll(long userId);
    Task<FilterResponseModel<CourseListViewModel>> GetAll();
    Task<CourseDetailViewModel> GetCourseDetails(long courseId);
    Task<bool> Delete(long Id);
    Task<bool> PublishCourse(long courseId);
    Task<long> GetCourseIdbyModule(long moduleId);
    Task<StudentCourseDetailsViewModel> GetStudentCourseDetails(long courseId, long userId);
    Task<CourseStatisticsResponse> GetCourseStatistics(long courseId);
    Task<bool> EnrollCourse(long courseId , long userId);
    Task<FilterResponseModel<MyLearningResponse>> MyCourses(long userId);
    Task<FilterResponseModel<MyLearningResponse>> GetFilteredMyCourses(long userId, Filter filter);
}