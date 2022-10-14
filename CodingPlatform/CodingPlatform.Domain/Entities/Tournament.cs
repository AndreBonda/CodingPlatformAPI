using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain.Entities;

public class Tournament : BaseEntity
{
    private const int _MIN_PARTICIPANTS = 2;
    [Required]
    public string Name { get; private set; }
    [Required]
    public int MaxParticipants { get; private set; }
    public User Admin { get; private set; }
    public ICollection<Challenge> Challenges { get; private set; }

    public Tournament(string name, int maxParticipants, User admin, ICollection<Challenge> challenges = null)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
        if (maxParticipants < _MIN_PARTICIPANTS) throw new ArgumentException(nameof(maxParticipants));
        
        Name = name;
        MaxParticipants = maxParticipants;
        Admin = admin;
        Challenges = challenges ?? new List<Challenge>();
    }
}