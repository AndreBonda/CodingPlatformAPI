using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserByEmailAsync(string email);
    Task<bool> ExistUserByEmailAsync(string email);
    Task<User> GetUserByUsernameAsync(string username);
    Task<bool> ExistUserByUsernameAsync(string username);

    Task<User> GetUserBySubmission(long submissionId);
    Task<User> GetAdminByChallenge(long challengeId);
    Task<User> GetTournamentAdminAsync(long tournamentId);
    Task<bool> IsUserAuthorizedToEvaluateSubmission(long userId, long submissionId);
    Task<IEnumerable<string>> GetSubscribedUsernamesAsync(long tournamentId);
}