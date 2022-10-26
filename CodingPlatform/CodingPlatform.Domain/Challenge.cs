using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain;

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

    private readonly List<Tip> _tips = new List<Tip>();
    public IReadOnlyCollection<Tip> Tips => _tips;

    private Challenge()
    {
    }

    public bool IsActive()
    {
        var now = DateTime.UtcNow;
        return CreateDate <= now && now <= EndDate;
    }

    public int TotalTips() => _tips.Count();

    private void SetTips(IEnumerable<string> tipDescriptions)
    {
        if (tipDescriptions == null) return;

        int count = 1;
        foreach (string tipDesc in tipDescriptions)
        {
            _tips.Add(new Tip(tipDesc, (byte)count));
            count++;
        }
    }

    public static Challenge CreateNew(string title, string description, int durationInHours, IEnumerable<string> tips = null)
    {
        if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));

        if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

        if (durationInHours < _MIN_CHALLENGE_DURATION || durationInHours > _MAX_CHALLENGE_DURATION)
            throw new ArgumentException(nameof(durationInHours));

        var challenge = new Challenge
        {
            Title = title,
            Description = description
        };

        challenge.EndDate = challenge.CreateDate.AddHours(durationInHours);
        challenge.SetTips(tips);

        return challenge;
    }
}