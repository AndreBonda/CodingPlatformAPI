using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.AppCore.Services;
using CodingPlatform.Domain;
using CodingPlatform.Domain.Entities;
using CodingPlatform.Domain.Exception;
using Moq;
using NUnit.Framework;

namespace CodingPlatform.AppCore.UnitTests;

[TestFixture]
public class TournamentServiceTests
{
    private ITournamentService _tournamentService;
    private Mock<ITournamentRepository> _tournamentRepository;
    private Mock<IUserRepository> _userRepository;
    private Mock<ISubmissionRepository> _submissionRepository;

    [SetUp]
    public void SetUp()
    {
        _tournamentRepository = new Mock<ITournamentRepository>();
        _userRepository = new Mock<IUserRepository>();
        _submissionRepository = new Mock<ISubmissionRepository>();
        _tournamentService = new TournamentService(_userRepository.Object, _tournamentRepository.Object,
            _submissionRepository.Object);
    }

    [Test]
    public void Create_NameAlreadyExist_ThrowBadRequestException()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetTournamentByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new Tournament("tournament",2,new User())));

        var exc = Assert.ThrowsAsync<BadRequestException>(() =>
            _tournamentService.Create(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<long>()));
        Assert.That(exc.Message, Does.Contain("name").IgnoreCase);
    }

    [Test]
    public async Task Create_WhenCalledCorrectly_StoreTheTournament()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetTournamentByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<Tournament>(null));
        _userRepository
            .Setup(userRepo => userRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new User()));

        await _tournamentService.Create("tournament1", 5, 2);
        
        _tournamentRepository.Verify(tourRepo => tourRepo.InsertAsync(It.IsAny<Tournament>()));
    }

    [Test]
    public void SubscribeUser_TournamentDoesNotExist_ThrowNotFoundException()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult<Tournament>(null));

        var exc = Assert.ThrowsAsync<NotFoundException>(
            () => _tournamentService.SubscribeUser(It.IsAny<long>(), It.IsAny<long>()));
        Assert.That(exc.Message, Does.Contain("tournament").IgnoreCase);
    }

    [Test]
    public void SubscribeUser_UserAlreadySubscribed_ThrowBadRequestException()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new Tournament("tournament",2,new User())));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.IsUserSubscribedAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(Task.FromResult(true));
        
        var exc = Assert.ThrowsAsync<BadRequestException>(
            () => _tournamentService.SubscribeUser(It.IsAny<long>(), It.IsAny<long>()));
        Assert.That(exc.Message, Does.Contain("user").IgnoreCase);
    }
    
    [Test]
    public void SubscribeUser_UserIsAdminOfTheTournament_ThrowBadRequestException()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new Tournament("tournament",2,new User())));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.IsUserSubscribedAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(Task.FromResult(false));
        _userRepository
            .Setup(userRepo => userRepo.GetTournamentAdminAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new User()
            {
                Id = 1
            }));
        
        var exc = Assert.ThrowsAsync<BadRequestException>(
            () => _tournamentService.SubscribeUser(It.IsAny<long>(), 1));
        Assert.That(exc.Message, Does.Contain("admin").IgnoreCase);
    }
    
    [Test]
    public void SubscribeUser_TournamentIsFull_ThrowBadRequestException()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new Tournament("tournament",2,new User())));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.IsUserSubscribedAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(Task.FromResult(false));
        _userRepository
            .Setup(userRepo => userRepo.GetTournamentAdminAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new User()
            {
                Id = 1
            }));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetSubscriberNumberAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(2));
        
        var exc = Assert.ThrowsAsync<BadRequestException>(
            () => _tournamentService.SubscribeUser(It.IsAny<long>(), 2));
        Assert.That(exc.Message, Does.Contain("full").IgnoreCase);
    }
    
    [Test]
    public async Task SubscribeUser_WhenCalledCorrectly_StoreSubscription()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new Tournament("tournament",4,new User())));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.IsUserSubscribedAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(Task.FromResult(false));
        _userRepository
            .Setup(userRepo => userRepo.GetTournamentAdminAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new User()
            {
                Id = 2
            }));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetSubscriberNumberAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(3));

        await _tournamentService.SubscribeUser(1, 3);

        _tournamentRepository
            .Verify(tourRepo => tourRepo.AddSubscriptionAsync(It.IsAny<Tournament>(), It.IsAny<User>()));
    }

    [Test]
    public void GetTournamentLeaderBoard_TournamentDoesNotExist_ThrowNotFoundException()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult<Tournament>(null));

        var exc = Assert.ThrowsAsync<NotFoundException>(() => _tournamentService.GetTournamentLeaderBoard(1));
        Assert.That(exc.Message, Does.Contain("tournament").IgnoreCase);
    }
    
    [Test]
    public async Task GetTournamentLeaderBoard_TournamentWithoutSubscribed_ReturnEmptyPositionsCollection()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Tournament("tournament",2,new User()));
        _userRepository
            .Setup(userRepo => userRepo.GetSubscribedUsernamesAsync(It.IsAny<long>()))
            .ReturnsAsync(new List<string>());
        _submissionRepository
            .Setup(subRepo => subRepo.GetSubmissionByTournament(It.IsAny<long>()))
            .ReturnsAsync(new List<Submission>());

        var positions = await _tournamentService.GetTournamentLeaderBoard(1);
        
        Assert.That(positions, Is.Not.Null);
        Assert.That(positions.Count(), Is.EqualTo(0));
    }
    
    [Test]
    public async Task GetTournamentLeaderBoard_SubscriberWithoutAnySubmission_AllScoresAreZero()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Tournament("tournament",2,new User()));
        _userRepository
            .Setup(userRepo => userRepo.GetSubscribedUsernamesAsync(It.IsAny<long>()))
            .ReturnsAsync(new List<string>()
            {
                "usernameA",
                "usernameB"
            });
        _submissionRepository
            .Setup(subRepo => subRepo.GetSubmissionByTournament(It.IsAny<long>()))
            .ReturnsAsync(new List<Submission>());

        var positions = await _tournamentService.GetTournamentLeaderBoard(1);

        Assert.That(positions, Is.Not.Null);
        Assert.That(positions.Count(), Is.EqualTo(2));
        var usernameAPosition = positions.Single(p => p.UserName == "usernameA");
        var usernameBPosition = positions.Single(p => p.UserName == "usernameB");
        Assert.That(usernameAPosition.TotalPoints, Is.EqualTo(0m));
        Assert.That(usernameBPosition.TotalPoints, Is.EqualTo(0m));
    }
    
    [Test]
    public async Task GetTournamentLeaderBoard_SubscriberWithMultipleSubmission_CorrectLeaderboardSetup()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Tournament("tournament",2,new User()));
        _userRepository
            .Setup(userRepo => userRepo.GetSubscribedUsernamesAsync(It.IsAny<long>()))
            .ReturnsAsync(new List<string>()
            {
                "usernameA",
                "usernameB"
            });
        _submissionRepository
            .Setup(subRepo => subRepo.GetSubmissionByTournament(It.IsAny<long>()))
            .ReturnsAsync(new List<Submission>()
            {
                new Submission()
                {
                    Score = 5,
                    User = new User()
                    {
                        UserName = "usernameA"
                    }
                },
                new Submission()
                {
                    Score = 4,
                    User = new User()
                    {
                        UserName = "usernameA"
                    }
                },
                new Submission()
                {
                    Score = 2,
                    User = new User()
                    {
                        UserName = "usernameB"
                    }
                }
            });

        var positions = await _tournamentService.GetTournamentLeaderBoard(1);

        Assert.That(positions, Is.Not.Null);
        Assert.That(positions.Count(), Is.EqualTo(2));
        var usernameAPosition = positions.ElementAt(0);
        var usernameBPosition = positions.ElementAt(1);
        Assert.That(usernameAPosition.TotalPoints, Is.EqualTo(9m));
        Assert.That(usernameAPosition.AveragePoints, Is.EqualTo(4.5m));
        Assert.That(usernameAPosition.Place, Is.EqualTo(1));
        Assert.That(usernameBPosition.TotalPoints, Is.EqualTo(2m));
        Assert.That(usernameBPosition.AveragePoints, Is.EqualTo(2m));
        Assert.That(usernameBPosition.Place, Is.EqualTo(2));
    }
}