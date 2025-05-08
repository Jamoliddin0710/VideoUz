using Application.DTOs;
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
    Task<CourseDetailViewModel> GetCourseDetails(long courseId);
    Task<bool> Delete(long Id);
    Task<bool> PublishCourse(long courseId);
}