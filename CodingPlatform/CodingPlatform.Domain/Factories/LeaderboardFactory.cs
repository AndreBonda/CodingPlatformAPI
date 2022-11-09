using System;
using System.Linq;
using CodingPlatform.Domain.Interfaces;
using static CodingPlatform.Domain.Leaderboard;

namespace CodingPlatform.Domain.Factories;

public class LeaderboardFactory : ILeaderboardFactory
{
    public LeaderboardFactory()
    {
    }

    public Leaderboard Create(IEnumerable<Submission> submissions)
    {
        if (submissions == null) throw new ArgumentNullException(nameof(submissions));

        var placements = submissions
            .GroupBy(s => s.User.Username)
            .Select(grouped => new Placement
            (
                grouped.First().User.Username,
                grouped.Count(),
                grouped.Sum(i => i.Score),
                grouped.Average(i => i.Score)
            ));

        return new Leaderboard(placements);
    }
}

