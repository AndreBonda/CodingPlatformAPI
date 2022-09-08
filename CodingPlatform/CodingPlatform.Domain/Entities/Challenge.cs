using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingPlatform.Domain.Entities;

public class Challenge : BaseEntity
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

    public ICollection<Tip> Tips { get; set; } = new List<Tip>();
    public ICollection<Submission> Submissions { get; set; }
    public Tournament Tournament { get; set; }
}