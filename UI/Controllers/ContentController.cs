using Application.DTOs;
using Application.Models;
using Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refit;
using UI.Services;

namespace UI.Controllers;

public class ContentController(
    IModuleRefitService moduleRefitService,
    IStorageRefitService storageRefitService,
    IContentRefitService contentRefitService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create(long moduleId)
    {
        var module = await moduleRefitService.GetById(moduleId);

        if (module == null)
            return NotFound();

        var viewModel = new ContentCreateViewModel
        {
            ModuleId = moduleId
        };

        ViewBag.ModuleTitle = module.Data?.Title ?? string.Empty;
        ViewBag.CourseTitle = module.Data?.Course?.Title ?? string.Empty;

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm]ContentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
           
            ViewBag.CourseId = TempData["CourseId"];
            ViewBag.CourseTitle = TempData["CourseTitle"];
            ViewBag.ModuleTitle = TempData["ModuleTitle"];
            return View(model);
        }
    
        var dto = model.Adapt<CreateContentDTO>();
    
      
        if (model.ContentType == ContentType.Text)
        {
            dto.FileId = null;
        }
        else
        {
      
            if (model.File == null || model.File.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file to upload");
                ViewBag.CourseId = TempData["CourseId"];
                ViewBag.CourseTitle = TempData["CourseTitle"];
                ViewBag.ModuleTitle = TempData["ModuleTitle"];
                return View(model);
            }
        
            using var stream = model.File.OpenReadStream();
            var streamPart = new StreamPart(stream, model.File.FileName, model.File.ContentType);
            var file = await storageRefitService.UploadFile(streamPart);
            dto.FileId = file?.Data?.Id;
        }
    
        await contentRefitService.Create(dto);
        return RedirectToAction("Details", "Module", new { id = model.ModuleId });
    }
}