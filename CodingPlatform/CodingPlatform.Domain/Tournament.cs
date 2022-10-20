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

    private readonly List<Subscription> _subscribedUser = new List<Subscription>();
    public IReadOnlyCollection<Subscription> SubscribedUser => _subscribedUser.AsReadOnly();

    private Tournament()
    { }

    public Tournament(string name, int maxParticipants, User admin)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));

        if (maxParticipants < _MIN_PARTICIPANTS) throw new ArgumentException(nameof(maxParticipants));

        if (admin == null) throw new ArgumentNullException(nameof(admin));

        Name = name;
        MaxParticipants = maxParticipants;
        Admin = admin;
    }

    public void AddSubscriber(User user)
    {
        if (user == null) throw new NotFoundException(nameof(user));
        if (user.Id == Admin.Id) throw new BadRequestException("An admin can't subscribe to his tournament");
        if (_subscribedUser.Any(s => s.User.Id == user.Id)) throw new BadRequestException("User already subscribed");
        if (_subscribedUser.Count() >= MaxParticipants) throw new BadRequestException("The tournament is full");

        _subscribedUser.Add(new Subscription(user));
    }

    public int SubscribedNumber => _subscribedUser.Count();

    public int AvailableSeats => MaxParticipants - SubscribedNumber;
}