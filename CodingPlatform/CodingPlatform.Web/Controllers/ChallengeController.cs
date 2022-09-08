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

    [HttpPost("challenge")]
    public async Task<IActionResult> CreateChallenge(CreateChallengeDto param)
    {
        var challenge = await _challengeService.CreateChallenge(param.TournamentId, param.Title, param.Description,
            param.Hours, GetCurrentUserId(), param.Tips);
        
        return Created(nameof(CreateChallenge), new CurrentChallengeDto()
        {
            Id = challenge.Id,
            Title = challenge.Title,
            Description = challenge.Description,
            StartDate = challenge.DateCreated,
            EndDate = challenge.EndDate,
            Tips = challenge.Tips?.Select(t => t.Description)
        });
    }
}