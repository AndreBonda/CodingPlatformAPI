using System.ComponentModel.DataAnnotations;
using static CodingPlatform.Domain.Leaderboard;

namespace CodingPlatform.Web.DTO;

public class LeaderboardPlacementDTO
{
    public int Position { get; set; }
    public string Username { get; set; }
    public int TotalChallenges { get; set; }
    public decimal TotalPoints { get; set; }
    public decimal AveragePoints { get; set; }
}

public class SearchLeaderboardDTO
{
    [Required][Range(1, long.MaxValue)] public long TournamentId { get; set; }
    [Required] public Sorting SortingType { get; set; }
    public bool SortiAsc { get; set; }
}