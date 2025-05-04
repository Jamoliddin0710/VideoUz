using System.IO.Pipelines;
using Application.DTOs;
using Application.ServiceContract;
using Application.Services;
using Domain.Entities.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class StorageController : BaseApiController
{
    private readonly IStorageService _storageService;

    public StorageController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<FileItem>>> UploadFile(IFormFile file)
    {
        var fileitem = await _storageService.UploadFileAsync(file);
        return Ok(fileitem);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Download(string bucket, string fileName)
    {
        try
        {
            var pipe = new Pipe();
            _storageService.DownloadStream(bucket, fileName, pipe.Writer);
            var fileSize = await _storageService.GetFileSizeAsync(bucket, fileName);
            Response.Headers.Append("Content-Length", fileSize.ToString());
            return new FileStreamResult(pipe.Reader.AsStream(), "application/octet-stream")
            {
                FileDownloadName = fileName,
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}