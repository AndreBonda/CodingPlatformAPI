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

    public async Task<User> GetUserBySubmission(long submissionId)
    {
        return (
            await dbCtx.Submissions
                .Include(s => s.User)
                .SingleAsync(s => s.Id == submissionId)
        ).User;
    }
    
    public async Task<User> GetAdminByChallenge(long challengeId)
    {
        return (
            await dbCtx.Challenges
                .Include(c => c.Tournament).ThenInclude(t => t.Admin)
                .FirstAsync(c => c.Id == challengeId)
        ).Tournament.Admin;
    }
    
    public async Task<User> GetTournamentAdminAsync(long tournamentId)
    {
        var tournament = await dbCtx.Set<Tournament>()
            .Include(t => t.Admin)
            .FirstOrDefaultAsync(x => x.Id == tournamentId);
        
        return tournament?.Admin;
    }
}