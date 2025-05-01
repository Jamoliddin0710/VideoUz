using Microsoft.AspNetCore.Http;

namespace Application.Services;

public interface IStorageService
{
        Task<string> UploadFileAsync(IFormFile file);
        Task<(byte[] FileContents, string ContentType, string FileName)> GetFileAsync(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
        Task<string> GetFileUrlAsync(string fileName);
    
}