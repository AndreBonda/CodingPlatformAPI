using CodingPlatform.Domain.Entities;

namespace CodingPlatform.Domain;

public class TournamentInfo
{
    public long Id { get; }
    public string Name { get; }
    public int MaxParticipants { get; }
    public string UserAdmin { get; }
    public int SubscriberNumber { get; }
    public DateTime DateDateCreated { get; }

    public TournamentInfo(long id, string name, int maxParticipants, string userAdmin, int subscriberNumber, DateTime dateCreated)
    {
        Id = id;
        Name = name;
        MaxParticipants = maxParticipants;
        UserAdmin = userAdmin;
        SubscriberNumber = subscriberNumber;
        DateDateCreated = dateCreated;
    }

    public int GetAvailableSeats()
    {
        return MaxParticipants - SubscriberNumber;
    }
}