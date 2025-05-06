using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.DTOs;

public class CourseCreateDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public long CategoryId { get; set; }
    public decimal  Price { get; set; }
    public long FileId { get; set; }
}

public class CourseDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public long CategoryId { get; set; }
    public decimal  Price { get; set; }
    public FileItemDTO CoverImage { get; set; }
}

public class FileItemDTO
{
    public long Id { get; set; }
    public string FileName { get; set; }
    public string Bucket { get; set; }
    public string StorageName { get; set; }
    public string Extension { get; set; }
}