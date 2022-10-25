using CodingPlatform.AppCore.Commands;
using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain;
using CodingPlatform.Domain.Exception;
using CodingPlatform.Domain.Extensions;

namespace CodingPlatform.AppCore.Services;

public class ChallengeService : IChallengeService
{
    private readonly ITournamentRepository _tournamentRepo;
    private readonly IChallengeRepository _challengeRepo;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IUserRepository _userRepo;

    public ChallengeService(ITournamentRepository tournamentRepository, IChallengeRepository challengeRepository,
        ISubmissionRepository submissionRepository, IUserRepository userRepository)
    {
        _tournamentRepo = tournamentRepository;
        _challengeRepo = challengeRepository;
        _submissionRepository = submissionRepository;
        _userRepo = userRepository;
    }

    public async Task<Challenge> CreateChallenge(CreateChallengeCmd cmd)
    {
        var tournament = await _tournamentRepo.GetByIdAsync(cmd.TournamentId);
        if (tournament == null) throw new NotFoundException(nameof(cmd.TournamentId));

        var user = await _userRepo.GetByIdAsync(cmd.UserId);
        var challenge = Challenge.CreateNew(cmd.Title,
                cmd.Description,
                cmd.Hours,
                cmd.Tips);
        tournament.AddChallenge(challenge, user);
        await _tournamentRepo.UpdateAsync(tournament);

        return challenge;
    }

    public async Task<IEnumerable<Challenge>> GetChallengesByUserAsync(long userId, bool onlyActive)
    {
        var subscribedTournaments = await _tournamentRepo.GetSubscribedTournamentsByUserAsync(userId);
        if (subscribedTournaments.Count() == 0) return Enumerable.Empty<Challenge>();

        var challenges = subscribedTournaments.SelectMany(t => t.Challenges);

        if (onlyActive)
            challenges = challenges.Where(c => c.IsActive());

        return challenges;
    }

    public async Task<Submission> StartChallengeAsync(long challengeId, long userId)
    {
        var tournament = await _tournamentRepo.GetTournamentByChallengeAsync(challengeId);
        if (tournament == null) throw new NotFoundException("Challenge does not exist");

        var user = await _userRepo.GetByIdAsync(userId);
        var submission = tournament.StartChallenge(user);
        return submission;
    }

    public async Task<SubmissionStatus> GetSubmissionStatus(long submissionId, long userId)
    {
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null) throw new NotFoundException("Submission does not exist");

        var user = await _userRepo.GetUserBySubmission(submissionId);
        if (userId != user.Id) throw new ForbiddenException("User is not authorized to this submission");

        var challenge = await _challengeRepo.GetChallengeBySubmission(submissionId);
        return new SubmissionStatus(submission, challenge, challenge.Tips);
    }

    public async Task<SubmissionStatus> AddSubmissionTip(long submissionId, long userId)
    {
        var subStatus = await GetSubmissionStatus(submissionId, userId);
        subStatus.AddTips();
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        submission.TipsNumber = (byte)subStatus.UsedTips;
        await _submissionRepository.UpdateAsync(submission);
        return subStatus;
    }

    public async Task<SubmissionStatus> EndSubmission(long submissionId, string content, long userId)
    {
        var subStatus = await GetSubmissionStatus(submissionId, userId);
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        subStatus.EndSubmission(content);
        submission.DateSubmitted = subStatus.SubmitDate;
        submission.Content = subStatus.Content;
        await _submissionRepository.UpdateAsync(submission);
        return subStatus;
    }

    public async Task<IEnumerable<Submission>> GetSubmissionsByChallenge(long challengeId, long userId)
    {
        var challenge = await _challengeRepo.GetByIdAsync(challengeId);
        if (challenge == null) throw new NotFoundException("Challenge does not exist");

        var admin = await _userRepo.GetAdminByChallenge(challengeId);
        if (userId != admin.Id) throw new ForbiddenException("User is not to this challenge");

        return await _submissionRepository.GetSubmissionsByChallengeAsync(challengeId);
    }

    public async Task<SubmissionStatus> EvaluateSubmission(long submissionId, int score, long userId)
    {
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null) throw new NotFoundException("Submission does not exist");

        if (!await _userRepo.IsUserAuthorizedToEvaluateSubmission(userId, submissionId))
            throw new ForbiddenException("User is not authorized to this submission");

        var challenge = await _challengeRepo.GetChallengeBySubmission(submissionId);
        var subStatus = new SubmissionStatus(submission, challenge, challenge.Tips);
        subStatus.SetScore(score);
        submission.Score = subStatus.Score;
        await _submissionRepository.UpdateAsync(submission);
        return subStatus;
    }
}