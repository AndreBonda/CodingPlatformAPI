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
    private readonly IUserRepository _userRepository;

    public ChallengeService(ITournamentRepository tournamentRepository, IChallengeRepository challengeRepository,
        ISubmissionRepository submissionRepository, IUserRepository userRepository)
    {
        _tournamentRepo = tournamentRepository;
        _challengeRepo = challengeRepository;
        _submissionRepository = submissionRepository;
        _userRepository = userRepository;
    }

    public async Task<Challenge> CreateChallenge(CreateChallengeCmd cmd)
    {
        var tournament = await _tournamentRepo.GetByIdAsync(cmd.TournamentId);
        if (tournament == null) throw new NotFoundException(nameof(cmd.TournamentId));

        var user = await _userRepository.GetByIdAsync(cmd.UserId);
        var challenge = Challenge.CreateNew(cmd.Title,
                cmd.Description,
                cmd.Hours,
                cmd.Tips);
        tournament.AddChallenge(challenge, user);
        await _tournamentRepo.UpdateAsync(tournament);

        return challenge;
    }

    public async Task<IEnumerable<Challenge>> GetChallengesByUser(long userId, bool onlyActive) =>
        await _challengeRepo.GetChallengesByUser(userId, onlyActive);

    public async Task<Submission> StartChallenge(long challengeId, long userId)
    {
        //    var challenge = await _challengeRepository.GetByIdAsync(challengeId);
        //    if (challenge == null) throw new NotFoundException("Challenge does not exist");

        //    var tournament = await _tournamentRepository.GetTournamentByChallengeAsync(challengeId);
        //    if (!await _tournamentRepository.IsUserSubscribedAsync(tournament.Id, userId))
        //        throw new BadRequestException("User is not subscribed to this tournament");

        //    if (!challenge.IsInProgress())
        //        throw new BadRequestException("Challenge is not in progress");

        //    var submission = await _submissionRepository.GetSubmissionByUserAndChallengeAsync(userId, challengeId);
        //    if (submission != null)
        //        throw new BadRequestException($"User has already started. Submission id: {submission.Id}");

        //    var user = await _userRepository.GetByIdAsync(userId);
        //    var newSubmission = new Submission()
        //    {
        //        DateSubmitted = null,
        //        Challenge = challenge,
        //        User = user,
        //        Content = string.Empty,
        //        TipsNumber = 0,
        //        Score = 0
        //    };
        //    newSubmission = await _submissionRepository.InsertAsync(newSubmission);

        //    return newSubmission;
        throw new Exception();
    }

    public async Task<SubmissionStatus> GetSubmissionStatus(long submissionId, long userId)
    {
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null) throw new NotFoundException("Submission does not exist");

        var user = await _userRepository.GetUserBySubmission(submissionId);
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

        var admin = await _userRepository.GetAdminByChallenge(challengeId);
        if (userId != admin.Id) throw new ForbiddenException("User is not to this challenge");

        return await _submissionRepository.GetSubmissionsByChallengeAsync(challengeId);
    }

    public async Task<SubmissionStatus> EvaluateSubmission(long submissionId, int score, long userId)
    {
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null) throw new NotFoundException("Submission does not exist");

        if (!await _userRepository.IsUserAuthorizedToEvaluateSubmission(userId, submissionId))
            throw new ForbiddenException("User is not authorized to this submission");

        var challenge = await _challengeRepo.GetChallengeBySubmission(submissionId);
        var subStatus = new SubmissionStatus(submission, challenge, challenge.Tips);
        subStatus.SetScore(score);
        submission.Score = subStatus.Score;
        await _submissionRepository.UpdateAsync(submission);
        return subStatus;
    }
}