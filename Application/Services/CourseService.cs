using Application.DTOs;
using Application.Models;
using Application.ServiceContract;
using Domain.Entities;
using Domain.RepositoryContracts;
using Mapster;

namespace Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public CourseService(IUnitOfWork unitOfWork, IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<CourseDTO> Create(CourseCreateDTO coursedto, long userId)
    {
        var course = coursedto.Adapt<Course>();
        course.AuthorId = userId;
        course.CoverImageId = coursedto.FileId;
        _unitOfWork.CourseRepo.Add(course);
        await _unitOfWork.CompleteAsync();
        return course.Adapt<CourseDTO>();
    }

    public Task Update(Course course)
    {
        throw new NotImplementedException();
    }

    public async Task<CourseDTO> GetbyId(long courseId)
    {
        var course = await _unitOfWork.CourseRepo.GetByIdAsync(courseId);
        return course.Adapt<CourseDTO>();
    }

    public async Task<FilterResponseModel<CourseListViewModel>> GetAll(long userId)
    {
        var courses =
            await _unitOfWork.CourseRepo.GetAllAsync(a => a.AuthorId == userId,
                includeProperties: "Modules,Enrollments,Category,CoverImage");
        
        var coverImageNames = courses
            .Select(x => x.CoverImage.StorageName)
            .Distinct()
            .ToList();

        var fileUrlDict = new Dictionary<string, string>();

        foreach (var name in coverImageNames)
        {
            var url = await _storageService.GetFileUrlAsync(name);
            fileUrlDict[name] = url;
        }

        var result = courses.Select(a => new CourseListViewModel()
        {
            Id = a.Id,
            Category = a.Category.Name,
            Description = a.Description,
            Title = a.Title,
            IsPublished = a.IsPublished,
            Price = a.Price,
            CreatedDate = a.CreatedDate,
            ModulesCount = a.Modules.Count(),
            StudentsCount = a.Enrollments.Count(),
            CoverImageUrl = fileUrlDict[a.CoverImage.StorageName],
        });

        return new FilterResponseModel<CourseListViewModel>()
        {
            Data = result,
            ItemsCount = result.Count()
        };
    }

    public async Task<CourseDetailViewModel> GetCourseDetails(long courseId)
    {
        var course = await _unitOfWork.CourseRepo.GetByIdAsync(courseId, includeProperties:"Category,Modules.Contents");
        var url = await _storageService.GetFileUrlAsync(course.CoverImage.StorageName);
        var result = new CourseDetailViewModel()
        {
            Id = course.Id,
            Category = course.Category.Adapt<CategoryViewModel>(),
            Description = course.Description,
            Title = course.Title,
            IsPublished = course.IsPublished,
            Price = course.Price,
            CreatedDate = course.CreatedDate,
            CoverImageUrl = url,
            Modules = course.Modules.Adapt<List<ModuleViewModel>>()
        };

        return result;
    }
}