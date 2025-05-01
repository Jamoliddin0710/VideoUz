using Microsoft.AspNetCore.Http;

namespace Application.ServiceContract;

public interface IPhotoService
{
    string UploadPhotoLocally(IFormFile photo, string oldPhotoUrl = "" , string webRootPath = "");
}