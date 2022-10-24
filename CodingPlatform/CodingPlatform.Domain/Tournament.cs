using System.ComponentModel.DataAnnotations;
using CodingPlatform.Domain.Exception;

namespace CodingPlatform.Domain;

public class Tournament : BaseEntity
{
    private const int _MIN_PARTICIPANTS = 2;

    [Required]
    public string Name { get; private set; }

    [Required]
    public int MaxParticipants { get; private set; }

    [Required]
    public User Admin { get; private set; }

    private readonly List<Subscription> _subscribedUser = new();
    public IReadOnlyCollection<Subscription> SubscribedUser => _subscribedUser;

    private readonly List<Challenge> _challenges = new();
    public IReadOnlyCollection<Challenge> Challenges => _challenges;

    private Tournament()
    { }

    public void AddSubscriber(User user)
    {
        if (user == null) throw new NotFoundException(nameof(user));
        if (IsUserAdmin(user)) throw new BadRequestException("An admin can't subscribe to his tournament");
        if (_subscribedUser.Any(s => s.User.Id == user.Id)) throw new BadRequestException("User already subscribed");
        if (_subscribedUser.Count() >= MaxParticipants) throw new BadRequestException("The tournament is full");

        _subscribedUser.Add(new Subscription(user));
    }

    public void AddChallenge(Challenge challenge, User user)
    {
        if (challenge == null) throw new ArgumentNullException(nameof(challenge));
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (!IsUserAdmin(user)) throw new BadRequestException("User is not the admin of the tournament");
        if (ChallengeAlreadyInProgress()) throw new BadRequestException("A challenge is already in progress");

        _challenges.Add(challenge);
    }

    public int SubscribedNumber => _subscribedUser.Count();

    public int AvailableSeats => MaxParticipants - SubscribedNumber;

    public bool IsUserAdmin(User user) => user?.Id == Admin.Id;

    public bool ChallengeAlreadyInProgress() => ChallengeInProgress() != null;

    public Challenge ChallengeInProgress() => _challenges.FirstOrDefault(c => c.IsActive());

    public static Tournament CreateNew(string name, int maxParticipants, User admin)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));

        if (maxParticipants < _MIN_PARTICIPANTS) throw new ArgumentException(nameof(maxParticipants));

        if (admin == null) throw new ArgumentNullException(nameof(admin));

        return new Tournament
        {
            Name = name,
            MaxParticipants = maxParticipants,
            Admin = admin
        };
    }
}