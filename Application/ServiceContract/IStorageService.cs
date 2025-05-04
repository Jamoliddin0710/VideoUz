using System.IO.Pipelines;
using Domain.Entities.Courses;
using Microsoft.AspNetCore.Http;

namespace Application.ServiceContract;

public interface IStorageService
{
        Task<FileItem> UploadFileAsync(IFormFile file);
        Task<(byte[] FileContents, string ContentType, string FileName)> GetFileAsync(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
        Task<string> GetFileUrlAsync(string fileName);
        void DownloadStream(string bucketName,  string fileName, PipeWriter pipeWrite);
        Task<long> GetFileSizeAsync(string bucketName,  string fileName);
}