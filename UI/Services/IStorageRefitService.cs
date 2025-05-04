using Application.DTOs;
using Domain.Entities.Courses;
using Refit;

namespace UI.Services;

public interface IStorageRefitService
{
    [Post("/storage/uploadfile")]
    [Multipart]
    Task<ServiceResponse<FileItem>> UploadFile([AliasAs("file")] StreamPart file);
}