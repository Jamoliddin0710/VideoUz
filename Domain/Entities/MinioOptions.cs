namespace Domain.Entities;

public class MinioOptions
{
    public string Scheme { get; set; }

    public string Host { get; set; }

    public string Endpoint => $"{Scheme}://{Host}";

    public string AccessKey { get; set; }

    public string SecretKey { get; set; }

    public string Service { get; set; } = "s3";

    public string Region { get; set; } = "us-east-1";
    public string Bucket { get; set; }
}