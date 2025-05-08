using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Courses;

namespace Domain.Entities;

public class Content : BaseEntity
{
    [Required] public long ModuleId { get; set; }

    [Required] [StringLength(200)] public string Title { get; set; }

    [Required] public ContentType ContentType { get; set; }

    public string?  ContentData { get; set; }
    public int Order { get; set; }

    public long? FileId { get; set; }
    [ForeignKey("FileId")] public virtual FileItem? FileItem { get; set; }
    public int? Duration { get; set; }
    [ForeignKey("ModuleId")] public virtual Module Module { get; set; }
    public virtual ICollection<Progress> Progresses { get; set; }
    public virtual ICollection<Quiz> Quizzes { get; set; }
}