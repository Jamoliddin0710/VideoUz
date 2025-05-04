using System.IO.Pipelines;
using Application.Models;
using Application.ServiceContract;
using Domain.Entities;
using Domain.Entities.Courses;
using Domain.RepositoryContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using IMinioClientFactory = Application.ServiceContract.IMinioClientFactory;

namespace Application.Services;

public class StorageService : IStorageService
{
    private readonly MinioOptions _minioOptions;
    private readonly IMinioClientFactory _minioClientFactory;
    private readonly IMinioClient minioClient;
    private readonly IUnitOfWork _unitOfWork;

    public StorageService(IOptions<MinioOptions> minioOptions, IMinioClientFactory minioClientFactory,
        IUnitOfWork unitOfWork)
    {
        _minioClientFactory = minioClientFactory;
        _unitOfWork = unitOfWork;
        minioClient = minioClientFactory.CreateClient();
        _minioOptions = minioOptions.Value;
    }

    public async Task<FileItem> UploadFileAsync(IFormFile file)
    {
        var bucketExists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_minioOptions.Bucket)
        ).ConfigureAwait(false);

        if (!bucketExists)
        {
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_minioOptions.Bucket)
            ).ConfigureAwait(false);
        }

        string storageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        using (var stream = file.OpenReadStream())
        {
            await minioClient.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(_minioOptions.Bucket)
                    .WithObject(storageName)
                    .WithContentType(file.ContentType)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
            ).ConfigureAwait(false);
        }

        var minioPath = $"{_minioOptions.Bucket}/{storageName}";
        var fileitem = new FileItem()
        {
            FileName = file.FileName,
            Bucket = _minioOptions.Bucket,
            Path = minioPath,
            Extention = Path.GetExtension(file.FileName),
            Size = file.Length,
            StorageName = storageName
        };
        _unitOfWork.FileRepo.Add(fileitem);
        await _unitOfWork.CompleteAsync();
        return fileitem;
    }

    public Task<(byte[] FileContents, string ContentType, string FileName)> GetFileAsync(string fileName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteFileAsync(string fileName)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetFileUrlAsync(string storageName)
    {
        var url = await minioClient.PresignedGetObjectAsync(
            new PresignedGetObjectArgs()
                .WithBucket(_minioOptions.Bucket)
                .WithObject(storageName)
                .WithExpiry(60 * 60)
        );

        return url;
    }


    public void DownloadStream(string bucketName, string fileName, PipeWriter pipeWrite)
    {
        _ = Task.Run(async () =>
        {
            await FileStream(bucketName, fileName, pipeWrite.AsStream());
            await pipeWrite.CompleteAsync();
        });
    }

    public async Task<long> GetFileSizeAsync(string bucketName, string fileName)
    {
        string objectName = fileName;
        StatObjectArgs statObjectArgs = new StatObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName);
        var stat = await minioClient.StatObjectAsync(statObjectArgs);
        return stat.Size;
    }

    private async Task FileStream(string bucketName, string fileName, Stream pipeStream)
    {
        try
        {
            string objectName = fileName;
            GetObjectArgs getObjectArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream((stream) => { stream.CopyTo(pipeStream); });
            await minioClient.GetObjectAsync(getObjectArgs);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}