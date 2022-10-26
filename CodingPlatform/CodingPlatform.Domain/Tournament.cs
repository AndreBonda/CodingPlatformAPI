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

    private readonly List<Submission> _submissions = new();
    public IReadOnlyCollection<Submission> Submissions => _submissions;

    private Tournament()
    { }

    public void AddSubscriber(User user)
    {
        if (user == null) throw new NotFoundException(nameof(user));
        if (IsUserAdmin(user)) throw new BadRequestException("An admin can't subscribe to his tournament.");
        if (IsUserSubscribed(user)) throw new BadRequestException("User already subscribed.");
        if (IsTournamentFull()) throw new BadRequestException("The tournament is full.");

        _subscribedUser.Add(new Subscription(user));
    }

    public void AddChallenge(Challenge challenge, User user)
    {
        if (challenge == null) throw new ArgumentNullException(nameof(challenge));
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (!IsUserAdmin(user)) throw new BadRequestException("User is not the admin of the tournament.");
        if (IsChallengeInProgress()) throw new BadRequestException("A challenge is already in progress for this tournament.");

        _challenges.Add(challenge);
    }

    public int SubscribedNumber() => _subscribedUser.Count();

    public int AvailableSeats() => MaxParticipants - SubscribedNumber();

    public bool IsTournamentFull() => AvailableSeats() == 0;

    public bool IsUserAdmin(User user) => user?.Id == Admin.Id;

    public bool IsChallengeInProgress() => GetChallengeInProgress() != null;

    public Challenge GetChallengeInProgress() => _challenges.FirstOrDefault(c => c.IsActive());

    public bool IsUserSubscribed(User user) => _subscribedUser.Any(s => user?.Id == s.User.Id);

    /// <summary>
    /// Returns true if the user has already started the challenge in progress for this tournament.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="challenge"></param>
    /// <returns></returns>
    public bool HasUserStartedChallenge(User user, Challenge challenge)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (challenge == null) throw new ArgumentNullException(nameof(challenge));

        return _submissions.Any(s => user.Id == s.User.Id && challenge.Id == s.Challenge.Id);
    }

    /// <summary>
    /// Start the challenge in progess for the tournament.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="BadRequestException"></exception>
    public Submission StartChallenge(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (IsUserAdmin(user)) throw new BadRequestException("An admin can't start a challenge.");

        if (!IsUserSubscribed(user)) throw new BadRequestException("User is not subscribed to this tournament.");

        var challenge = GetChallengeInProgress();
        if (challenge == null) throw new BadRequestException("A challenge is not in progress for this tournament");

        if (HasUserStartedChallenge(user, challenge))
            throw new BadRequestException("User already started the challenge");

        var submission = Submission.CreateNew(user, Admin, challenge);
        _submissions.Add(submission);
        return submission;
    }

    public Submission GetSubmissionStatus(User user, long challengeId)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        var submission = _submissions
            .FirstOrDefault(s => s.Challenge.Id == challengeId && s.User.Id == user.Id);

        return submission ?? throw new NotFoundException("No submission found");
    }

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