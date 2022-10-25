using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain;

public class Submission : BaseEntity
{
    public byte TipsNumber { get; private set; }
    public DateTime? DateSubmitted { get; private set; }
    public string Content { get; private set; }
    public decimal Score { get; private set; }
    [Required]
    public User User { get; private set; }
    [Required]
    public User Admin { get; private set; }
    [Required]
    public Challenge Challenge { get; private set; }

    public static Submission CreateNew(User user, User admin, Challenge challenge)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (admin == null) throw new ArgumentNullException(nameof(admin));

        if (challenge == null) throw new ArgumentNullException(nameof(challenge));

        return new Submission
        {
            TipsNumber = 0,
            Content = String.Empty,
            Score = 0,
            User = user,
            Admin = admin,
            Challenge = challenge
        };
    }
}