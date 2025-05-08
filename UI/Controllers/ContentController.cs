using Application.DTOs;
using Application.Models;
using Domain.Entities;
using Infrastructure.DTOs;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Refit;
using UI.Services;
namespace UI.Controllers;
public class ContentController(
    IModuleRefitService moduleRefitService,
    IStorageRefitService storageRefitService,
    ICourseRefitService courseRefitService,
    IOptions<AppOptions> options,
    HttpClient _httpClient,
    IContentRefitService contentRefitService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create(long moduleId)
    {
        var module = await moduleRefitService.GetById(moduleId);

        if (module is null)
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
    public async Task<IActionResult> Create([FromForm] ContentCreateViewModel model)
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
        var course = await courseRefitService.GetcourseByModule(model.ModuleId);
        return RedirectToAction("Details", "Course", new { id = course.Data.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var content = await contentRefitService.GetById(id);
        if (!content.IsSuccessful)
        {
            TempData["Error"] = "Module not found";
            return RedirectToAction("Index", "Course");
        }
        
        var editViewModel = new ContentEditViewModel()
        {
            Id = content.Data.Id,
            Title = content.Data.Title,
            ModuleId = content.Data.ModuleId,
            ContentType = content.Data.ContentType,
            ContentData = content.Data.ContentData,
            FileName = content.Data.FileItem.StorageName,
            Bucket = content.Data.FileItem.Bucket
        };
        return View(editViewModel);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(long id)
    {
        await contentRefitService.Delete(id);
        return Ok();
    }
    
    private string GetContentType(string fileName)
    {
        return Path.GetExtension(fileName).ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream"
        };
    }
    
    private async Task<(Stream Stream, string FileName, string ContentType)> DownloadFileAsStreamAsync(string bucket,
        string fileName)
    {
        var apiUrl =
            $"{options.Value.BackendApi!.TrimEnd('/')}/storage/download?bucket={bucket}&fileName={fileName}";
        var response = await _httpClient.GetAsync(apiUrl, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
            throw new Exception("File read error");

        var stream = await response.Content.ReadAsStreamAsync();
        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
        var contentDisposition = response.Content.Headers.ContentDisposition;
        var name = contentDisposition?.FileName?.Trim('"') ?? fileName;

        return (stream, name, contentType);
    }

}