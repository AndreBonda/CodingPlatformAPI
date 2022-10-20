using System;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodingPlatform.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaderboardController : CustomControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(IHttpContextAccessor httpCtxAccessor, ILeaderboardService leaderboardService)
            : base(httpCtxAccessor)
        {
            _leaderboardService = leaderboardService;
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard([FromQuery] SearchLeaderboardDTO param)
        {
            var placements = await _leaderboardService.GetLeaderboardPlacements(
                param.TournamentId,
                param.SortingType,
                param.SortiAsc);

            return Ok(placements.Select(p => new LeaderboardPlacementDTO()
            {
                Position = p.Key,
                Username = p.Value.Username,
                TotalPoints = p.Value.TotalPoints,
                AveragePoints = p.Value.AveragePoints,
                TotalChallenges = p.Value.TotalChallenges
            }));
        }
    }
}

