using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodingPlatform.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChallengeController : CustomControllerBase
{
    private readonly IChallengeService _challengeService;
    
    public ChallengeController(IHttpContextAccessor httpContextAccessor, IChallengeService challengeService) 
        : base(httpContextAccessor)
    {
        _challengeService = challengeService;
    }
    
    [HttpGet("challenges_in_progress")]
    public async Task<IActionResult> UserInProgressChallenges()
    {
        var userInProgressChallenges = await _challengeService.GetActiveChallengesByUser(GetCurrentUserId());
        
        return Ok(userInProgressChallenges.Select(c => new InfoInProgressChallengeDto
        {
            ChallengeId = c.Id,
            ChallengeName = c.Title,
            DateStart = c.DateCreated,
            DateEnd = c.EndDate,
            TournamentName = c.Tournament.Name
        }));
    }

    [HttpPost("challenge")]
    public async Task<IActionResult> CreateChallenge(CreateChallengeDto param)
    {
        var challenge = await _challengeService.CreateChallenge(param.TournamentId, param.Title, param.Description,
            param.Hours, GetCurrentUserId(), param.Tips);
        
        return Created(nameof(CreateChallenge), new ChallengeDto()
        {
            Id = challenge.Id,
            Title = challenge.Title,
            Description = challenge.Description,
            StartDate = challenge.DateCreated,
            EndDate = challenge.EndDate,
            Tips = challenge.Tips?.Select(t => t.Description)
        });
    }

    [HttpPost("challenge_start/{challengeId}")]
    public async Task<IActionResult> StartChallenge(long challengeId)
    {
        var submission = await _challengeService.StartChallenge(challengeId, GetCurrentUserId());
        return Created(nameof(StartChallenge), new
        {
            IdSubmission = submission.Id,
            Started = submission.DateCreated
        });
    }

    [HttpGet("submission_status/{submissionId}")]
    public async Task<IActionResult> GetSubmissionStatus(long submissionId)
    {
        var subStatus = await _challengeService.GetSubmissionStatus(submissionId, GetCurrentUserId());
        return Ok(new SubmissionStatusDto
        {
            SubmissionId = subStatus.SubmissionId,
            Started = subStatus.StartDate,
            EndDate = subStatus.EndDate,
            Submitted = subStatus.SubmitDate,
            ChallengeTitle = subStatus.ChallengeTitle,
            ChallengeDescription = subStatus.ChallengeDescription,
            Content = subStatus.Content,
            Score = subStatus.Score,
            ChallengeTipAvailableNumber = subStatus.ChallengeTipAvailableNumber(),
            UsedTips = subStatus.GetUsedTips().ToArray(),
            RemainingTipsNumber = subStatus.RemainingTipsNumber()
        });
    }

    [HttpPost("submission_add_tip/{submissionId}")]
    public async Task<IActionResult> AddSubmissionTip(long submissionId)
    {
        var subStatus = await _challengeService.AddSubmissionTip(submissionId, GetCurrentUserId());
        
        return Created(nameof(AddSubmissionTip), new SubmissionStatusDto
        {
            SubmissionId = subStatus.SubmissionId,
            Started = subStatus.StartDate,
            EndDate = subStatus.EndDate,
            Submitted = subStatus.SubmitDate,
            ChallengeTitle = subStatus.ChallengeTitle,
            ChallengeDescription = subStatus.ChallengeDescription,
            Content = subStatus.Content,
            Score = subStatus.Score,
            ChallengeTipAvailableNumber = subStatus.ChallengeTipAvailableNumber(),
            UsedTips = subStatus.GetUsedTips().ToArray(),
            RemainingTipsNumber = subStatus.RemainingTipsNumber()
        });
    }

    [HttpPost("submission_end/{submissionId}")]
    public async Task<IActionResult> EndSubmission(long submissionId, [FromBody] string content)
    {
        var subStatus = await _challengeService.EndSubmission(submissionId, content, GetCurrentUserId());
        
        return Created(nameof(AddSubmissionTip), new SubmissionStatusDto
        {
            SubmissionId = subStatus.SubmissionId,
            Started = subStatus.StartDate,
            EndDate = subStatus.EndDate,
            Submitted = subStatus.SubmitDate,
            ChallengeTitle = subStatus.ChallengeTitle,
            ChallengeDescription = subStatus.ChallengeDescription,
            Content = subStatus.Content,
            Score = subStatus.Score,
            ChallengeTipAvailableNumber = subStatus.ChallengeTipAvailableNumber(),
            UsedTips = subStatus.GetUsedTips().ToArray(),
            RemainingTipsNumber = subStatus.RemainingTipsNumber()
        });
    }

    [HttpGet("submissions/{challengeId}")]
    public async Task<IActionResult> GetSubmissions(long challengeId)
    {
        var submissions = 
            await _challengeService.GetSubmissionsByChallenge(challengeId, GetCurrentUserId());
        return Ok(submissions.Select(s => new SubmissionDto
        {
            SubmissionId = s.Id,
            Content = s.Content,
            Score = s.Score,
            Started = s.DateCreated,
            Submitted = s.DateSubmitted
        }));
    }
}