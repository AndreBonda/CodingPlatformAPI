using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain;

public abstract class BaseEntity
{
    [Key]
    public long Id { get; set; }
    [Required]
    public DateTime DateCreated { get; set; }
}