using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.AppCore.Services;
using CodingPlatform.Domain.Entities;
using CodingPlatform.Domain.Exception;
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
            .Returns(Task.FromResult(new Tournament()));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetTournamentAdminAsync(It.IsAny<long>()))
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
            .Returns(Task.FromResult(new Tournament()));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetTournamentAdminAsync(It.IsAny<long>()))
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
        var tournament = new Tournament();
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(tournament));
        _tournamentRepository
            .Setup(tourRepo => tourRepo.GetTournamentAdminAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(new User()
            {
                Id = 1
            }));
        _challengeRepository
            .Setup(challRepo => challRepo.GetActiveChallengeByTournament(
                It.IsAny<long>(), It.IsAny<DateTime>()))
            .Returns(Task.FromResult<Challenge>(null));

        var challenge = await _challengeService.CreateChallenge(1, "title", "desc", 1, 1,
            new[] {"tip1", "tip2"});
        
        _tournamentRepository
            .Verify(tourRepo => tourRepo.UpdateAsync(tournament));
        Assert.That(challenge.EndDate, Is.GreaterThan(challenge.DateCreated));
        Assert.That(challenge.Tips, Is.Not.Null);
        Assert.That(challenge.Tips.Count, Is.EqualTo(2));
        Assert.That(challenge.Tips.ElementAt(0).Order, Is.EqualTo(1));
    }
    
    
}