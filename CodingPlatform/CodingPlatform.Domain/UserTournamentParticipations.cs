namespace CodingPlatform.Domain;

public class UserTournamentParticipations : BaseEntity
{
    public Tournament Tournament { get; set; }
    public User User { get; set; }
}