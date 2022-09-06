namespace CodingPlatform.Domain.Entities;

public class UserTournamentParticipations : BaseEntity
{
    public Tournament Tournament { get; set; }
    public User User { get; set; }
}