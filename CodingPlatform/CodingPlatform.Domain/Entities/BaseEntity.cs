using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public long Id { get; set; }
    [Required]
    public DateTime DateCreated { get; set; }
}