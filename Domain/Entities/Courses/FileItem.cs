namespace Domain.Entities.Courses;

public class FileItem : BaseEntity
{
    public string? Bucket { get; set; }

    public string? Path { get; set; }

    public string? StorageName { get; set; }

    public string? FileName { get; set; }

    public string? Extention { get; set; }

    public long? Size { get; set; }
}