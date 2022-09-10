using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Entities;
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
            Started = submission.DateCreated
        });
    }

    [HttpGet("submission_status/{challengeId}")]
    public async Task<IActionResult> GetSubmissionStatus(long challengeId)
    {
        var subStatus = await _challengeService.GetSubmissionStatus(challengeId, GetCurrentUserId());
        return Ok(new SubmissionStatusDto
        {
            Started = subStatus.StartDate,
            EndDate = subStatus.EndDate,
            Submitted = subStatus.SubmitDate,
            Content = subStatus.Content,
            Score = subStatus.Score,
            TotalAvailableTips = subStatus.TotalAvailableTips,
            TipsUsed = subStatus.TipsUsed.ToArray(),
            TipsUsedNumber = subStatus.TipsUsedNumber(),
            RemainingTips = subStatus.RemainingTips()
        });
    }

}