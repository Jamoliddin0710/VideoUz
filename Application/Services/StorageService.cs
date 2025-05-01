using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Application.Services;

public class StorageService : IStorageService
{
    private readonly MinioOptions _minioOptions;
    private readonly MinioClientFactory _minioClientFactory;
    public StorageService(IOptions<MinioOptions> minioOptions, MinioClientFactory minioClientFactory)
    {
        _minioClientFactory = minioClientFactory;
        _minioOptions = minioOptions.Value;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var minioClient = _minioClientFactory.CreateClient();
       var bucketExists = await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_minioOptions.Bucket));
       if (!bucketExists)
       {
           await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_minioOptions.Bucket));
       }

       string fileName = $"{Guid.NewGuid()}{file.FileName}";
       using (var stream = file.OpenReadStream())
       {
           await minioClient.PutObjectAsync(
               new PutObjectArgs()
                   .WithBucket(_minioOptions.Bucket)
                   .WithObject(fileName)
                   .WithContentType(file.ContentType)
                   .WithStreamData(stream)
                   .WithObjectSize(stream.Length));
       }

       return fileName;
    }

    public Task<(byte[] FileContents, string ContentType, string FileName)> GetFileAsync(string fileName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteFileAsync(string fileName)
    {
          throw new NotImplementedException();
    }

    public Task<string> GetFileUrlAsync(string fileName)
    {
        throw new NotImplementedException();
    }
}