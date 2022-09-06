using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain.Entities;

public class Tournament : BaseEntity
{
    public string Name { get; set; }
    [Required]
    public int MaxParticipants { get; set; }
    public User Admin { get; set; }
}