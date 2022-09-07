namespace CodingPlatform.Domain;

public class TournamentInfo
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int MaxParticipants { get; set; }
    public string UserAdmin { get; set; }
    public int SubscriberNumber { get; set; }
    public DateTime DateCreated { get; set; }

    public int GetAvailableSeats()
    {
        return MaxParticipants - SubscriberNumber;
    }
}