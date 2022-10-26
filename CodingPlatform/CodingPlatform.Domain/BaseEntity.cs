using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain;

public abstract class BaseEntity
{
    [Key]
    public long Id { get; private set; }
    [Required]
    public DateTime CreateDate { get; private set; }

    public BaseEntity()
    {
        CreateDate = DateTime.UtcNow;
    }
}