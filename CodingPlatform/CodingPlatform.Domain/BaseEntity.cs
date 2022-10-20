using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain;

public abstract class BaseEntity
{
    [Key]
    public long Id { get; private set; }
    [Required]
    public DateTime DateCreated { get; private set; }

    public BaseEntity()
    {
        DateCreated = DateTime.UtcNow;
    }
}