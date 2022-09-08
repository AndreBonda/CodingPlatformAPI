using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface IChallengeRepository : IRepository<Challenge>
{
    Task<IEnumerable<Challenge>> GetActiveChallengesByUser(long userId);
}