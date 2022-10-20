using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.AppCore.Services;
using CodingPlatform.Domain.Exception;
using CodingPlatform.Domain.Extensions;
using Moq;
using NUnit.Framework;

namespace CodingPlatform.AppCore.UnitTests;

[TestFixture]
public class ChallengeServiceTests
{
    /*
     * assert _tournamentRepository.UpdateAsync is called
     */

    private ChallengeService _challengeService;
    private Mock<ITournamentRepository> _tournamentRepository;
    private Mock<IChallengeRepository> _challengeRepository;
    private Mock<ISubmissionRepository> _submissionRepository;
    private Mock<IUserRepository> _userRepository;

    [SetUp]
    public void SetUp()
    {
        _tournamentRepository = new();
        _challengeRepository = new();
        _submissionRepository = new();
        _userRepository = new();
        _challengeService = new ChallengeService(
            _tournamentRepository.Object,
            _challengeRepository.Object,
            _submissionRepository.Object,
            _userRepository.Object);
    }

    [Test]
    public void CreateChallenge_TournamentDoesNotExist_ThrowNotFoundException()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult<Tournament>(null));

        var exc = Assert.ThrowsAsync<NotFoundException>(
            () => _challengeService.CreateChallenge(1, "title", "desc", 1, 1, null));
        Assert.That(exc.Message, Does.Contain("tournament").IgnoreCase);
    }

    [Test]
    public void CreateChallenge_UserIsNotAdminOfTheTournament_ThrowForbiddenException()
    {
        int requestUser = 1;
        int adminUser = 2;
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new Tournament("tournament", 2, new User())));
        _userRepository
            .Setup(userRepo => userRepo.GetTournamentAdminAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new User()
            {
                Id = adminUser
            }));

        var exc = Assert.ThrowsAsync<ForbiddenException>(
            () => _challengeService.CreateChallenge(requestUser, "title", "desc", 1, 1, null));
        Assert.That(exc.Message, Does.Contain("create").IgnoreCase);
    }

    [Test]
    public void CreateChallenge_ThereIsInProgressChallengeForTheTournament_ThrowBadRequestException()
    {
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new Tournament("tournament", 2, new User())));
        _userRepository
            .Setup(userRepo => userRepo.GetTournamentAdminAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new User()
            {
                Id = 1
            }));
        _challengeRepository
            .Setup(challRepo => challRepo.GetActiveChallengeByTournament(
                It.IsAny<long>(), It.IsAny<DateTime>()))
            .Returns(Task.FromResult(new Challenge()));

        var exc = Assert.ThrowsAsync<BadRequestException>(
            () => _challengeService.CreateChallenge(1, "title", "desc", 1, 1, null));
        Assert.That(exc.Message, Does.Contain("progress").IgnoreCase);
    }

    [Test]
    public async Task CreateChallenge_WhenCalledCorrectly_StoreChallenge()
    {
        var tournament = new Tournament("tournament", 2, new User());
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(tournament));
        _userRepository
            .Setup(userRepo => userRepo.GetTournamentAdminAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new User()
            {
                Id = 1
            }));
        _challengeRepository
            .Setup(challRepo => challRepo.GetActiveChallengeByTournament(
                It.IsAny<long>(), It.IsAny<DateTime>()))
            .Returns(Task.FromResult<Challenge>(null));

        var challenge = await _challengeService.CreateChallenge(1, "title", "desc", 1, 1,
            new[] { "tip1", "tip2" });

        _tournamentRepository
            .Verify(tourRepo => tourRepo.UpdateAsync(tournament));
        Assert.That(challenge.EndDate, Is.GreaterThan(challenge.DateCreated));
        Assert.That(challenge.Tips, Is.Not.Null);
        Assert.That(challenge.Tips.Count, Is.EqualTo(2));
        Assert.That(challenge.Tips.ElementAt(0).Order, Is.EqualTo(1));
        Assert.That(challenge.Tips.ElementAt(1).Order, Is.EqualTo(2));
        Assert.That(challenge.Tips.ElementAt(0).Description, Is.EqualTo("tip1"));
        Assert.That(challenge.Tips.ElementAt(1).Description, Is.EqualTo("tip2"));
    }

    [Test]
    public void StartChallenge_ChallengeDoesNotExist_ThrowBadRequestException()
    {
        _challengeRepository
            .Setup(challRepo => challRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult<Challenge>(null));

        var exc = Assert.ThrowsAsync<NotFoundException>(
            () => _challengeService.StartChallenge(1, 1));
        Assert.That(exc.Message, Does.Contain("exist").IgnoreCase);
    }

    [Test]
    public void StartChallenge_UserIsNotSubscribedToTheTournament_ThrowBadRequestException()
    {
        _challengeRepository
            .Setup(challRepo => challRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new Challenge()));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetTournamentByChallengeAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new Tournament("tournament", 2, new User())));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.IsUserSubscribedAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(Task.FromResult(false));

        var exc = Assert.ThrowsAsync<BadRequestException>(
            () => _challengeService.StartChallenge(1, 1));
        Assert.That(exc.Message, Does.Contain("subscribed").IgnoreCase);
    }

    [Test]
    public void StartChallenge_ChallengeIsNotInProgress_ThrowBadRequestException()
    {
        var challenge = new Challenge()
        {
            DateCreated = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(-1)
        };
        _challengeRepository
            .Setup(challRepo => challRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(challenge));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetTournamentByChallengeAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new Tournament("tournament", 2, new User())));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.IsUserSubscribedAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(Task.FromResult(true));

        var exc = Assert.ThrowsAsync<BadRequestException>(
            () => _challengeService.StartChallenge(1, 1));
        Assert.That(exc.Message, Does.Contain("progress").IgnoreCase);
    }

    [Test]
    public void StartChallenge_UserHasAlreadyStarted_ThrowBadRequestException()
    {
        var challenge = new Challenge()
        {
            DateCreated = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(1)
        };
        _challengeRepository
            .Setup(challRepo => challRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(challenge));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetTournamentByChallengeAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new Tournament("tournament", 2, new User())));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.IsUserSubscribedAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(Task.FromResult(true));
        _submissionRepository
            .Setup(subRepo =>
                subRepo.GetSubmissionByUserAndChallengeAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(Task.FromResult(new Submission()));


        var exc = Assert.ThrowsAsync<BadRequestException>(
            () => _challengeService.StartChallenge(1, 1));
        Assert.That(exc.Message, Does.Contain("started").IgnoreCase);
    }

    [Test]
    public async Task StartChallenge_WhenCalledCorrectly_StoreSubmission()
    {
        var challenge = new Challenge()
        {
            DateCreated = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(1)
        };
        _challengeRepository
            .Setup(challRepo => challRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(challenge));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetTournamentByChallengeAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new Tournament("tournament", 2, new User())));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.IsUserSubscribedAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(Task.FromResult(true));
        _submissionRepository
            .Setup(subRepo =>
                subRepo.GetSubmissionByUserAndChallengeAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(Task.FromResult<Submission>(null));
        _userRepository
            .Setup(userRepo => userRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new User()));
        _submissionRepository
            .Setup(subRepo => subRepo.InsertAsync(It.IsAny<Submission>()))
            .Returns(Task.FromResult(new Submission()));

        await _challengeService.StartChallenge(1, 1);

        _submissionRepository
            .Verify(subRepo => subRepo.InsertAsync(
                It.Is<Submission>(s =>
                    s.TipsNumber == 0 && s.DateSubmitted == null && s.Content == string.Empty && s.Score == 0)));

    }
}