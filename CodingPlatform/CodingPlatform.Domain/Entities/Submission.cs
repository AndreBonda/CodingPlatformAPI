using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain.Entities;

public class Submission : BaseEntity
{
    public byte TipsNumber { get; set; }
    public DateTime? DateSubmitted { get; set; }
    public string Content { get; set; }

    public decimal Score { get; set; }
    
    [Required]
    public Challenge Challenge { get; set; }
    [Required]
    public User User { get; set; }
}