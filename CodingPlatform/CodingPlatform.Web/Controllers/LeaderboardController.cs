using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodingPlatform.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaderboardController : CustomControllerBase
    {
        public LeaderboardController(IHttpContextAccessor httpCtxAccessor)
            : base(httpCtxAccessor)
        {
        }

        [HttpGet("leaderboard/{tournamentId}")]
        public async Task<IActionResult> GetLeaderboard(long tournamentId)
        {
            throw new NotImplementedException();
        }
    }
}

