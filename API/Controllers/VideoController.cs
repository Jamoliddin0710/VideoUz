using Application.DTOs;
using Application.Helpers;
using Application.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;

namespace API.Controllers;

public class VideoController : BaseApiController
{
    private FileUploadOption _fileUploadOption;
    private readonly IVideoService _videoService;
    public VideoController(IOptions<FileUploadOption> options, IVideoService videoService)
    {
        _videoService = videoService;
        _fileUploadOption = options.Value;
    }
    /*public async Task<IActionResult> Create(CreateOrEditVideoDTO model)
    {
        var userid = User.GetUserId();
        if (!userid.HasValue)
        {
            return Unauthorized();
        }

        return Ok(default);
    }*/
}