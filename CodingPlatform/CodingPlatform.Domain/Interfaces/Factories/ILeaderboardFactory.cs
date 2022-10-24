namespace CodingPlatform.Domain.Interfaces;

public interface ILeaderboardFactory
{
    Leaderboard Create(IEnumerable<Submission> submissions);
}