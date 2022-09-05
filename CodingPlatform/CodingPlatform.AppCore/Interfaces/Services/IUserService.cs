using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface IUserService
{
    Task<User> InsertUserEncryptingPassword(User user, string plainTextPassword);
}