using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Application.ServiceContract;
using Domain.Entities;
using Domain.RepositoryContracts;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly UserManager<AppUser> _userManager;

    public CourseService(IUnitOfWork unitOfWork, IStorageService storageService, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _userManager = userManager;
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
        var course = await _unitOfWork.CourseRepo.GetByIdAsync(courseId, includeProperties: "CoverImage");
        return course.Adapt<CourseDTO>();
    }

    public async Task<CourseDTO> GetcourseByModule(long moduleId)
    {
        var course = await _unitOfWork.CourseRepo.GetFirstOrDefaultAsync(a => a.Modules.Any(a => a.Id == moduleId));
        return course.Adapt<CourseDTO>();
    }

    public async Task<FilterResponseModel<CourseListViewModel>> GetAll(long userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var roles = await _userManager.GetRolesAsync(user);
        var courses = new List<Course>();
        if (roles.Any(a => a == nameof(Role.Teacher)))
        {
            courses =
                (await _unitOfWork.CourseRepo.GetAllAsync(a => a.AuthorId == userId,
                    includeProperties: "Modules,Enrollments,Category,CoverImage")).ToList();
        }
        else
        {
            courses = (await _unitOfWork.CourseRepo.GetAllAsync(a => a.IsPublished,
                includeProperties: "Modules,Enrollments,Category,CoverImage")).ToList();
        }

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
    
    public async Task<FilterResponseModel<CourseListViewModel>> GetAll()
    {
        var courses = (await _unitOfWork.CourseRepo.GetAllAsync(a => a.IsPublished,
            includeProperties: "Modules,Enrollments,Category,CoverImage,Author")).ToList();
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
            User = a.Author.Name,
            CoverImageUrl = fileUrlDict[a.CoverImage.StorageName],
        });

        return new FilterResponseModel<CourseListViewModel>()
        {
            Data = result,
            ItemsCount = result.Count()
        };
    }
    
    public async Task<FilterResponseModel<MyLearningResponse>> MyCourses(long userId)
    {
        var courseIds = (await _unitOfWork.EnrollmentRepo.GetAllAsync(a => a.UserId == userId)).Select(a => a.CourseId)
            .ToList();
        var courses = await _unitOfWork.CourseRepo.GetAllAsync(a => a.IsPublished && courseIds.Contains(a.Id),
            includeProperties: "Author,CoverImage");
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

        var result = courses.Select(a => new MyLearningResponse()
        {
            Id = a.Id,
            Name = a.Title,
            Author = a.Author.Name,
            CoverImage = fileUrlDict[a.CoverImage.StorageName]
        });
        return new FilterResponseModel<MyLearningResponse>()
        {
            Data = result,
            ItemsCount = result.Count()
        };
    }

    public async Task<FilterResponseModel<MyLearningResponse>> GetFilteredMyCourses(long userId, Filter filter)
    {
        var courseIds = (await _unitOfWork.EnrollmentRepo.GetAllAsync(a => a.UserId == userId)).Select(a => a.CourseId)
            .ToList();
        var courses = await _unitOfWork.CourseRepo.GetAllAsync(a => a.IsPublished && courseIds.Contains(a.Id),
            includeProperties: "Author,CoverImage");
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

        var result = courses.Select(a => new MyLearningResponse()
        {
            Id = a.Id,
            Name = a.Title,
            Author = a.Author.Name,
            CoverImage = fileUrlDict[a.CoverImage.StorageName]
        }).AsQueryable().ApplyFilter(filter);

        return new FilterResponseModel<MyLearningResponse>()
        {
            Data = result,
            ItemsCount = result.Count(),
        };
    }

    public async Task<CourseDetailViewModel> GetCourseDetails(long courseId)
    {
        var course = await _unitOfWork.CourseRepo.GetByIdAsync(courseId,
            includeProperties: "CoverImage,Category,Modules.Contents.FileItem");
        if (course is null)
            throw new ApplicationException("Course doesn't exists ");

        var result = new CourseDetailViewModel()
        {
            Modules = new List<ModuleViewModel>(),
        };

        result.Id = course.Id;
        if (!string.IsNullOrWhiteSpace(course?.CoverImage?.StorageName))
        {
            result.CoverImageUrl = await _storageService.GetFileUrlAsync(course.CoverImage.StorageName);
        }
        
        result.Category = course.Category.Adapt<CategoryViewModel>();
        result.Title = course.Title;
        result.Description = course.Description;
        result.IsPublished = course.IsPublished;
        result.Price = course.Price;
        result.CreatedDate = course.CreatedDate;

        foreach (var module in course.Modules)
        {
            var moduleViewModel = new ModuleViewModel()
            {
                Contents = new(),
            };
            moduleViewModel.Id = module.Id;
            moduleViewModel.Title = module.Title;
            moduleViewModel.Description = module.Description;
            moduleViewModel.Order = module.Order;
            foreach (var content in module.Contents)
            {
                var contentViewModel = new ContentViewModel();
                contentViewModel.Id = content.Id;
                contentViewModel.Title = content.Title;
                contentViewModel.QuizId = content.Quizzes.FirstOrDefault()?.Id;
                contentViewModel.ContentType = content.ContentType;
                if (!string.IsNullOrWhiteSpace(content?.FileItem?.StorageName))
                {
                    contentViewModel.ContentUrl = await _storageService.GetFileUrlAsync(content.FileItem.StorageName);
                }
                else
                {
                    contentViewModel.ContentUrl = string.Empty;
                }

                moduleViewModel.Contents.Add(contentViewModel);
            }

            result.Modules.Add(moduleViewModel);
        }

        return result;
    }

    public async Task<bool> Delete(long Id)
    {
        var course = await _unitOfWork.CourseRepo.GetByIdAsync(Id);
        _unitOfWork.CourseRepo.Remove(course);
        return await _unitOfWork.CompleteAsync();
    }

    public async Task<bool> PublishCourse(long courseId)
    {
        var course = await _unitOfWork.CourseRepo.GetByIdAsync(courseId);
        if (course is null)
            throw new Exception("course doesn't exists");
        course.IsPublished = true;
        return await _unitOfWork.CompleteAsync();
    }

    public async Task<long> GetCourseIdbyModule(long moduleId)
    {
        return (await _unitOfWork.ModuleRepo.GetFirstOrDefaultAsync(a => a.Id == moduleId)).CourseId;
    }

    public async Task<StudentCourseDetailsViewModel> GetStudentCourseDetails(long courseId, long userId)
    {
        var course = await _unitOfWork.CourseRepo
            .GetFirstOrDefaultAsync(a => a.Id == courseId, includeProperties: "Category,Enrollments,Modules,Author");

        return new StudentCourseDetailsViewModel()
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Price = course.Price,
            IsEnrolled = course.Enrollments.Any(a => a.UserId == userId),
            Category = course.Category.Adapt<CategoryViewModel>(),
            CoverImageUrl = string.Empty,
            Progress = new CourseProgressDTO(),
            Modules = course.Modules.Adapt<List<StudentModuleViewModel>>()
        };
    }

    public async Task<CourseStatisticsResponse> GetCourseStatistics(long courseId)
    {
        var course = await _unitOfWork.CourseRepo.GetFirstOrDefaultAsync(a => a.Id == courseId,
            includeProperties: "Category,Enrollments,Modules.Contents,Author");
        return new CourseStatisticsResponse()
        {
            ModuleCount = course.Modules.Count,
            ContentCount = course.Modules.Sum(a => a.Contents.Count),
            Price = course.Price,
        };
    }

    public async Task<bool> EnrollCourse(long courseId, long userId)
    {
        var enroll = new Enrollment()
        {
            CourseId = courseId,
            UserId = userId,
            EnrollDate = DateTime.Now,
            Status = EnrollmentStatus.Active
        };
        _unitOfWork.EnrollmentRepo.Add(enroll);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}