using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserByUsername(string username);
    Task<User> GetUserBySubmission(long submissionId);
    /// <summary>
    /// Return the admin of a challenge.
    /// </summary>
    /// <param name="challengeId"></param>
    /// <returns></returns>
    Task<User> GetAdminByChallenge(long challengeId);
    Task<User> GetTournamentAdminAsync(long tournamentId);
    Task<bool> IsUserAuthorizedToEvaluateSubmission(long userId, long submissionId);
    Task<IEnumerable<string>> GetSubscribedUsernamesAsync(long tournamentId);
}