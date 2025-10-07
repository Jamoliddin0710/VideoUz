using Domain.Entities;
using Microsoft.Extensions.Options;
using Minio;
using IMinioClientFactory = Application.ServiceContract.IMinioClientFactory;

namespace Application.Services;

public class MinioClientFactory : IMinioClientFactory
{
    private readonly MinioOptions _options;

    public MinioClientFactory(IOptions<MinioOptions> options)
    {
        _options = options.Value;
    }

    public IMinioClient CreateClient()
    {
        bool useSSL = _options.Host.StartsWith("https://");
        string endpoint = _options.Host.Replace("http://", "").Replace("https://", "");

        return new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(_options.AccessKey, _options.SecretKey)
            .WithSSL(useSSL) 
            .Build();
    }
    
}