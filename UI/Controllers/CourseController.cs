using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Application.Helpers;
using Application.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refit;
using UI.Models;
using UI.Services;

namespace UI.Controllers;

public class CourseController : Controller
{
    private readonly ICourseRefitService _courseRefitService;
    private readonly ICategoryRefitService _categoryRefitService;
    private readonly IStorageRefitService _storageRefitService;
    public CourseController(ICourseRefitService courseRefitService, ICategoryRefitService categoryRefitService, IStorageRefitService storageRefitService)
    {
        _courseRefitService = courseRefitService;
        _categoryRefitService = categoryRefitService;
        _storageRefitService = storageRefitService;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CourseCreateViewModel();
        ViewBag.Categories = await GetAllCategoryDropDown();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await GetAllCategoryDropDown();
            return View(model);
        }
        try
        {
            using var stream = model.CoverImage.OpenReadStream();
            var streamPart = new StreamPart(stream, model.CoverImage.FileName, model.CoverImage.ContentType);
            var fileResponse = await _storageRefitService.UploadFile(streamPart);
            
            var courseDto = new CourseCreateDTO
            {
                Title = model.Title,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Price = model.Price,
                FileId = fileResponse.Data.Id 
            };

            var result = await _courseRefitService.Create(courseDto);
            TempData["SuccessMessage"] = "Course created successfully! Now add modules to your course.";
            return RedirectToAction("Create", "Module", new { courseId = result.Data.Id });

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.Categories = await GetAllCategoryDropDown();
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _courseRefitService.GetAll();
        
        if (!response.IsSuccessful)
        {
            TempData["Error"] = response.Error.Message;
            return View(new List<CourseListViewModel>());
        }

        var viewModels = response.Data.Data.ToList();

        return View(viewModels);
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(long id)
    {
        var response = await _courseRefitService.GetCourseWithDetails(id);

        if (!response.IsSuccessful)
        {
            TempData["Error"] = "Course not found";
            return RedirectToAction(nameof(Index));
        }

        var course = response.Data;
        return View(course);
    }
    
    private async Task<List<SelectListItem>> GetAllCategoryDropDown()
    {
        var categories = await _categoryRefitService.GetAllCategories(new Filter());
        var result = categories?.Data?.Data?.ToList() ?? new List<CategoryDTO>();
        return result.Select(a => new SelectListItem()
        {
            Text = a.Name,
            Value = a.Id.ToString(),
        }).ToList();
    }
}