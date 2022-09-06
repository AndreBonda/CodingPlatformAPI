using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbCtx) : base(dbCtx)
    {
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await dbCtx.Set<User>()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User> GetUserByUsername(string username)
    {        
        return await dbCtx.Set<User>()
        .FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
    }
}