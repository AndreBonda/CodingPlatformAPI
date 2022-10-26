using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface IChallengeRepository : IRepository<Challenge>
{
    Task<IEnumerable<Challenge>> GetChallengesByUser(long userId, bool onlyActive);
}