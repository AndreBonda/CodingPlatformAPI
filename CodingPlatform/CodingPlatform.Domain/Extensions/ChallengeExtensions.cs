namespace CodingPlatform.Domain.Extensions;

public static class ChallengeExtensions
{
    public static bool IsInProgress(this Challenge challenge)
    {
        var now = DateTime.UtcNow;
        return challenge.DateCreated <= now && challenge.EndDate >= now;
    }
}