using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain.Entities;

public class User : BaseEntity
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public byte[] PasswordSalt { get; set; }
    [Required]
    public byte[] PasswordHash { get; set; }
    
    public ICollection<Tournament> TournamentsAdmin { get; set; }
    public ICollection<UserTournamentParticipations> UserTournamentParticipations { get; set; }
    public ICollection<Submission> Submissions { get; set; }
}