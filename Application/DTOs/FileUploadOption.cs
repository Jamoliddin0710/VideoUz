namespace Application.DTOs;

public class FileUploadOption
{
    public int ImageMaxSizeInMB { get; set; }
    public List<string> ImageContentTypes { get; set; }
    public string VideoMaxSizeInMB { get; set; }
    public List<string> VideoContentTypes { get; set; }
}