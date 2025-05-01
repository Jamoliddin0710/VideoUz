using Application.DTOs;
using Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;

namespace API.Controllers;

public class VideoController : BaseApiController
{
    private FileUploadOption _fileUploadOption;
    public VideoController(IOptions<FileUploadOption> options)
    {
        _fileUploadOption = options.Value;
    }
    /*public async Task<IActionResult> CreateOrEditVideo(CreateOrEditVideoDTO model)
    {
        var userid = User.GetUserId();
        if (!userid.HasValue)
        {
            return Unauthorized();
        }
        return View
    }*/
}