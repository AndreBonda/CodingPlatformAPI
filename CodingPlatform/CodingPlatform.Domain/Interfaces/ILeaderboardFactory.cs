namespace CodingPlatform.Domain.Interfaces;

public interface ILeaderboardFactory
{
    Leaderboard GetInstance(IEnumerable<Submission> submissions);
}