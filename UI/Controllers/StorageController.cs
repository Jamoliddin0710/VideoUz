using Application.ServiceContract;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace UI.Controllers;

public class StorageController(IOptions<AppOptions> options, HttpClient _httpClient) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Download(string bucket, string fileName)
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
}