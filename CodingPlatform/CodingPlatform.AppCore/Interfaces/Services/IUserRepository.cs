using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserByUsername(string username);
}