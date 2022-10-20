using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbCtx) : base(dbCtx)
    {
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _dbCtx.Set<User>()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await _dbCtx.Set<User>()
        .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<User> GetUserBySubmission(long submissionId)
    {
        return (
            await _dbCtx.Submissions
                .Include(s => s.User)
                .SingleAsync(s => s.Id == submissionId)
        ).User;
    }

    public async Task<User> GetAdminByChallenge(long challengeId)
    {
        return (
            await _dbCtx.Challenges
                .Include(c => c.Tournament).ThenInclude(t => t.Admin)
                .FirstAsync(c => c.Id == challengeId)
        ).Tournament.Admin;
    }

    public async Task<User> GetTournamentAdminAsync(long tournamentId)
    {
        var tournament = await _dbCtx.Set<Tournament>()
            .Include(t => t.Admin)
            .FirstOrDefaultAsync(x => x.Id == tournamentId);

        return tournament?.Admin;
    }

    public async Task<bool> IsUserAuthorizedToEvaluateSubmission(long userId, long submissionId)
    {
        var sub = await _dbCtx.Submissions
            .Include(s => s.Challenge)
            .ThenInclude(c => c.Tournament)
            .ThenInclude(t => t.Admin)
            .FirstAsync(s => s.Id == submissionId);
        return sub.Challenge.Tournament.Admin.Id == userId;
    }

    public async Task<IEnumerable<string>> GetSubscribedUsernamesAsync(long tournamentId)
    {
        //return await _dbCtx.UserTournamentParticipations
        //    .Include(utp => utp.User)
        //    .Where(utp => utp.Tournament.Id == tournamentId)
        //    .Select(utp => utp.User.UserName)
        //    .ToListAsync();

        throw new NotImplementedException();
    }
}