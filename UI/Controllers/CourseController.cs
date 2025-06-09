using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Refit;
using UI.Models;
using UI.Services;

namespace UI.Controllers;

public class CourseController(
    ICourseRefitService _courseRefitService,
    ICategoryRefitService _categoryRefitService,
    IStorageRefitService _storageRefitService,
    HttpClient _httpClient,
    IOptions<AppOptions> options) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CourseCreateViewModel();
        ViewBag.Categories = await GetAllCategoryDropDown();
        return View(model);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(long id)
    {
        await _courseRefitService.Delete(id);
        return Ok();
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
        var statistics = await _courseRefitService.GetStatistics(id);
        if (!response.IsSuccessful)
        {
            TempData["Error"] = "Course not found";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.statistics = statistics.Data;
        var course = response.Data;
        return View(course);
    }


    [HttpGet]
    public async Task<IActionResult> DownloadFile(string bucket, string fileName)
    {
        var apiUrl =
            $"{options.Value.BackendApi!.TrimEnd('/')}/storage/download?bucket={bucket}&fileName={fileName}";
        var response = await _httpClient.GetAsync(apiUrl, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "file read error");
        }

        var stream = await response.Content.ReadAsStreamAsync();
        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
        var fileSize = response.Content.Headers.ContentLength;
        if (fileSize > 0)
        {
            Response.Headers.Append("Content-Length", fileSize.ToString());
        }

        var contentDisposition = response.Content.Headers.ContentDisposition;
        var Name = contentDisposition?.FileName?.Trim('"') ?? "downloadedFile";
        return new FileStreamResult(stream, contentType)
        {
            FileDownloadName = Name
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


    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var course = await _courseRefitService.GetById(id);
        if (!course.IsSuccessful)
        {
            TempData["Error"] = "Module not found";
            return RedirectToAction("Index", "Course");
        }

        IFormFile formFile = null;
        try
        {
            var (stream, fileName, contentType) = await DownloadFileAsStreamAsync(
                course.Data.CoverImage.Bucket,
                course.Data.CoverImage.StorageName);

            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            formFile = new FormFile(memoryStream, 0, memoryStream.Length, "CoverImage", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = GetContentType(course.Data.CoverImage.FileName)
            };
        }
        catch
        {
            TempData["Error"] = "Cover image load failed.";
        }

        ViewBag.Categories = await GetAllCategoryDropDown();
        var editViewModel = new CourseEditViewModel
        {
            Id = course.Data.Id,
            Title = course.Data.Title,
            Description = course.Data.Description,
            CategoryId = course.Data.CategoryId,
            Price = course.Data.Price,
            CoverImage = formFile
        };

        return View(editViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> PublishCourse(long id)
    {
        await _courseRefitService.PublishCourse(id);
        return Ok();
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

    [HttpGet]
    public async Task<IActionResult> EnrollmentCourse(long courseId)
    {
        await _courseRefitService.EnrolleCourse(courseId);
        return RedirectToAction("MyLearning");
    }

    [HttpGet]
    public async Task<IActionResult> MyLearning()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetMyCourses([FromBody] Filter filter)
    {
        var result = await _courseRefitService.GetFilteredMyCourses(filter);
        return Json(new APIResponse(200, result: result.Data.Data));
    }

    [HttpGet]
    public async Task<IActionResult> MyCourse(long id)
    {
        var response = await _courseRefitService.GetCourseWithDetails(id);
        return View(response.Data);
    }
}