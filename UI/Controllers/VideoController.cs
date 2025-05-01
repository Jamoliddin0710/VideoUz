using Application.DTOs;
using Application.Helpers;
using Application.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Services;

namespace UI.Controllers;

public class VideoController : Controller
{
    private readonly IChannelRefitService _channelRefitService;
    private readonly ICategoryRefitService _categoryRefitService;
    private readonly IConfiguration _configuration;
    public VideoController(IChannelRefitService channelRefitService, ICategoryRefitService categoryRefitService, IConfiguration configuration)
    {
        _channelRefitService = channelRefitService;
        _categoryRefitService = categoryRefitService;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> CreateEditVideo(long id = 0)
    {
        var userId = User.GetUserId();
        if (userId.HasValue && !(await _channelRefitService.UsersChannelExists()).Data)
        {
            TempData["notication"] = "false; Not found; No channel assosiated with your account was found";
            return RedirectToAction("Index", "Channel");
        }

        var createOrEditVideoDto = new CreateOrEditVideoDTO();
        createOrEditVideoDto.ImageContentTypes = string.Join(",", AcceptableContentTypes("image"));
        createOrEditVideoDto.VideoContentTypes = string.Join(",", AcceptableContentTypes("video"));
        createOrEditVideoDto.CategoryDropDown = await GetAllCategoryDropDown();
        return View(createOrEditVideoDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEditVideo(CreateOrEditVideoDTO model)
    {
       
             if (ModelState.IsValid)
             {
                 bool proceed = true;
 
                 if (model.Id == 0)
                 {
                     // adding some security check for create
                     if (model.ImageFile == null)
                     {
                         ModelState.AddModelError("ImageUpload", "Please upload thumbnail");
                         proceed = false;
                     }
 
                     if (proceed && model.VideoFile == null)
                     {
                         ModelState.AddModelError("VideoUpload", "Please upload your video");
                         proceed = false;
                     }
                 }
 
                 if (model.ImageFile != null)
                 {
                     if (proceed && !IsAcceptableContentType("image", model.ImageFile.ContentType))
                     {
                         ModelState.AddModelError("ImageUpload", string.Format("Invalid content type. It must be one of the following: {0}",
                             string.Join(", ", AcceptableContentTypes("image"))));
                         proceed = false;
                     }
 
                     if (proceed && model.ImageFile.Length > int.Parse(_configuration["FileUpload:ImageMaxSizeInMB"]) * SD.MB)
                     {
                         ModelState.AddModelError("ImageUpload", string.Format("The uploaded file should not exceed {0} MB",
                             int.Parse(_configuration["FileUpload:ImageMaxSizeInMB"])));
                         proceed = false;
                     }
                 }
 
                 if (model.VideoFile != null)
                 {
                     if (proceed && !IsAcceptableContentType("video", model.VideoFile.ContentType))
                     {
                         ModelState.AddModelError("VideoUpload", string.Format("Invalid content type. It must be one of the following: {0}",
                             string.Join(", ", AcceptableContentTypes("video"))));
                         proceed = false;
                     }
 
                     if (proceed && model.VideoFile.Length > int.Parse(_configuration["FileUpload:VideoMaxSizeInMB"]) * SD.MB)
                     {
                         ModelState.AddModelError("VideoUpload", string.Format("The uploaded file should not exceed {0} MB",
                             int.Parse(_configuration["FileUpload:VideoMaxSizeInMB"])));
                         proceed = false;
                     }
                 }
 
                 if (proceed)
                 {
                     string title = "";
                     string message = "";
 
                     if (model.Id == 0)
                     {
                         /*// for create
                         var videoToAdd = new Video()
                         {
                             Title = model.Title,
                             Description = model.Description,
                             ContentType = model.VideoFile.ContentType,
                             Contents = GetContentsAsync(model.VideoFile).GetAwaiter().GetResult(),
                             CategoryId = model.CategoryId,
                             ChannelId = UnitOfWork.ChannelRepo.GetChannelIdByUserId(User.GetUserId()).GetAwaiter().GetResult(),
                             ThumbnailUrl = _photoService.UploadPhotoLocally(model.ImageUpload)
                         };
 
                         UnitOfWork.VideoRepo.Add(videoToAdd);*/
 
                         title = "Created";
                         message = "New video has been created";
                     }
                     else
                     {
                         // for update
                         /*var fetchedVideo = await UnitOfWork.VideoRepo.GetByIdAsync(model.Id);
                         if (fetchedVideo == null)
                         {
                             TempData["notification"] = "false;Not Found;Requested video was not found";
                             return RedirectToAction("Index", "Channel");
                         }
 
                         fetchedVideo.Title = model.Title;
                         fetchedVideo.Description = model.Description;
                         fetchedVideo.CategoryId = model.CategoryId;
 
                         if (model.ImageFile != null)
                         {
                             fetchedVideo.ThumbnailUrl = _photoService.UploadPhotoLocally(model.ImageUpload, fetchedVideo.ThumbnailUrl);
                         }*/
 
                         title = "Edited";
                         message = "Video has been updated";
                     }
 
                     TempData["notification"] = $"true;{title};{message}";
                     /*await UnitOfWork.CompleteAsync();*/
 
                     return RedirectToAction("Index", "Channel");
                 }
             }
 
             model.CategoryDropDown = await GetAllCategoryDropDown();
             return View(model);
    }

    private async Task<List<SelectListItem>> GetAllCategoryDropDown()
    {
        var categories = await _categoryRefitService.GetAllCategories(new Filter());
        var result =  categories?.Data?.Data?.ToList() ?? new List<CategoryDTO>();
        return result.Select(a => new SelectListItem()
        {
            Text = a.Name,
            Value = a.Id.ToString(),
        }).ToList();
    }
    
    private string[] AcceptableContentTypes(string type)
    {
        if (type.Equals("image"))
        {
            return _configuration.GetSection("FileUpload:ImageContentTypes").Get<string[]>();
        }
        else
        {
            return _configuration.GetSection("FileUpload:VideoContentTypes").Get<string[]>();
        }
    }
 
    private bool IsAcceptableContentType(string type, string contentType)
    {
        var allowedContentTypes = AcceptableContentTypes(type);
        foreach (var allowedContentType in allowedContentTypes)
        {
            if (contentType.ToLower().Equals(allowedContentType.ToLower()))
            {
                return true;
            }
        }
 
        return false;
    }
    
}