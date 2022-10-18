using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain;

public class Tournament : BaseEntity
{
    private const int _MIN_PARTICIPANTS = 2;
    [Required]
    public string Name { get; private set; }
    [Required]
    public int MaxParticipants { get; private set; }
    public User Admin { get; private set; }
    public ICollection<Challenge> Challenges { get; private set; }

    private Tournament()
    {
        Challenges = new List<Challenge>();
    }

    public Tournament(string name, int maxParticipants, User admin, ICollection<Challenge> challenges = null)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
        if (maxParticipants < _MIN_PARTICIPANTS) throw new ArgumentException(nameof(maxParticipants));
        
        Name = name;
        MaxParticipants = maxParticipants;
        Admin = admin;
        Challenges = challenges ?? new List<Challenge>();
    }

    public void AddChallenge(Challenge newChallenge)
    {
        if (newChallenge == null) throw new ArgumentNullException(nameof(newChallenge));
        Challenges.Add(newChallenge);
    }
}