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

    public async Task<IEnumerable<User>> GetFiltered(UserFilters f)
    {
        IQueryable<User> results = dbCtx.Set<User>();

        if (!string.IsNullOrWhiteSpace(f.Email))
            results = results.Where(u => u.Email.ToLower().Contains(f.Email.ToLower()));

        if (!string.IsNullOrWhiteSpace(f.Username))
            results = results.Where(u => u.UserName.ToLower().Contains(f.Username.ToLower()));

        results = results.Take(f.Take)
            .Skip(f.Page * f.Take);

        return await results.ToListAsync();
    }
}