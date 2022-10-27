using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbCtx) : base(dbCtx)
    {
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        if (email == null) throw new ArgumentNullException(nameof(email));
        return await _dbCtx.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower()); ;
    }

    public async Task<bool> ExistUserByEmailAsync(string email) => await GetUserByEmailAsync(email) != null;

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _dbCtx.Set<User>()
        .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<bool> ExistUserByUsernameAsync(string username) => await GetUserByUsernameAsync(username) != null;

}