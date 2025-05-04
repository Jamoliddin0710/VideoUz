using Application.DTOs;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using UI.Models;
using UI.Services;

namespace UI.Controllers;

public class ModuleController : Controller
{
    private readonly IModuleRefitService _moduleService;
    private readonly ICourseRefitService _courseService;

    public ModuleController(IModuleRefitService moduleService, ICourseRefitService courseService)
    {
        _moduleService = moduleService;
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> Create(long courseId)
    {
        var course = await _courseService.GetById(courseId);
        if (course.Data == null)
        {
            return NotFound();
        }

        var existingModules = await _moduleService.GetModulesByCourses(courseId);
        int nextOrder = 1;

        if (existingModules.IsSuccessful && existingModules.Data != null)
        {
            nextOrder = existingModules.Data.ItemsCount + 1;
        }

        var model = new ModuleCreateViewModel
        {
            CourseId = courseId,
            Order = nextOrder
        };

        ViewBag.CourseName = course.Data.Title;
        ViewBag.ModuleNumber = nextOrder;
        ViewBag.ExistingModules = existingModules.Data.Data;

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ModuleCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var course = await _courseService.GetById(model.CourseId);
            ViewBag.CourseName = course.Data.Title;
            return View(model);
        }

        try
        {
            var moduleDto = new CreateModuleDTO()
            {
                CourseId = model.CourseId,
                Title = model.Title,
                Description = model.Description,
                Order = model.Order
            };

            var response = await _moduleService.Create(moduleDto);

            if (response.IsSuccessful)
            {
                TempData["SuccessMessage"] = $"Module '{model.Title}' created successfully!";

                return RedirectToAction("Create", "Content", new { moduleId = response.Data.Id });
            }

            ModelState.AddModelError("", response.Error.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }
}