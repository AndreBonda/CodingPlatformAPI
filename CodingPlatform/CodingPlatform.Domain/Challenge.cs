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

    private List<Tip> _tips;
    public Tournament Tournament { get; private set; }

    private Challenge()
    {
    }

    public Challenge(string title, string description, int durationInHours, Tournament tournament, IEnumerable<string> tips = null)
    {
        if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));

        if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

        if (durationInHours < _MIN_CHALLENGE_DURATION || durationInHours > _MAX_CHALLENGE_DURATION)
            throw new ArgumentException(nameof(durationInHours));

        if (tournament == null) throw new ArgumentNullException(nameof(tournament));

        Title = title;
        Description = description;
        EndDate = DateCreated.AddHours(durationInHours);
        SetTips(tips);
    }

    public void SetTips(IEnumerable<string> tips)
    {
        _tips = new List<Tip>();
        if (tips == null || tips.Count() == 0) return;

        int count = 1;
        foreach (string tipDesc in tips)
        {
            _tips.Add(new Tip(tipDesc, (byte)count));
            count++;
        }
    }

    public IEnumerable<Tip> Tips => _tips.AsReadOnly();
}