using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain.Entities;

public class CurrentChallenge : BaseEntity
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

    public ICollection<Tip> Tips { get; set; } = new List<Tip>();
}