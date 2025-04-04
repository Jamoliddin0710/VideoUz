using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class BaseEntity : AuditEntity
{
    [Key]
    public long Id { get; set; }
}