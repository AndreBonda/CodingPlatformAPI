using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain.Entities;

public class Challenge : BaseEntity
{
    private const int _MIN_CHALLENGE_DURATION = 1;
    private const int _MAX_CHALLENGE_DURATION = 3;

    [Required]
    public string Title { get; private set; }
    [Required]
    public string Description { get; private set; }
    [Required]
    public DateTime EndDate { get; private set; }
    public ICollection<Tip> Tips { get; private set; }
    // public ICollection<Submission> Submissions { get; private set; }
    public Tournament Tournament { get; private set; }

    private Challenge()
    {
    }

    public Challenge(string title, string description, int durationInHours, Tournament tournament, IEnumerable<string> tips )
    {
        if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));
        if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));
        if (durationInHours < _MIN_CHALLENGE_DURATION || durationInHours > _MAX_CHALLENGE_DURATION)
            throw new ArgumentException(nameof(durationInHours));

        if (tournament == null) throw new ArgumentNullException(nameof(tournament));
        
        Title = title;
        Description = description;
        var now = DateTime.UtcNow;
        DateCreated = now;
        EndDate = now.AddHours(durationInHours);
        
        //TODO: tips setup
    }
}