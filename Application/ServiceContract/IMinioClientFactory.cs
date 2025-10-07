using Minio;

namespace Application.ServiceContract;

public interface IMinioClientFactory
{
    IMinioClient CreateClient();
}