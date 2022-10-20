using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public class TournamentRepository : BaseRepositoryRefactor<Tournament>, ITournamentRepository
{
    public TournamentRepository(AppDbContext dbCtx) : base(dbCtx)
    {
    }

    public override async Task<Tournament> GetByIdAsync(long id)
    {
        return await _dbCtx.Tournaments
            .Include(t => t.SubscribedUser)
            .ThenInclude(s => s.User)
            .Include(t => t.Admin)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Tournament>> GetFilteredAsync(TournamentSearch f = null)
    {
        f ??= new TournamentSearch();
        IQueryable<Tournament> results = _dbCtx.Set<Tournament>()
            .Include(t => t.SubscribedUser)
            .ThenInclude(s => s.User)
            .Include(t => t.Admin);

        if (!string.IsNullOrWhiteSpace(f.TournamentName))
            results = results.Where(t => t.Name.ToLower().Contains(f.TournamentName.ToLower()));

        //results = SetPagination(results, f);

        return await results.ToListAsync();
    }

    public async Task<Tournament> GetTournamentByNameAsync(string name)
    {
        if (name == null || string.IsNullOrEmpty(name)) return null;

        return await _dbCtx.Set<Tournament>()
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
    }

    //public async Task<bool> IsUserSubscribedAsync(long tournamentId, long userId)
    //{
    //    return await _dbCtx.Set<UserTournamentParticipations>()
    //        .AnyAsync(utp => utp.Tournament.Id == tournamentId && utp.User.Id == userId);
    //}

    //public async Task<UserTournamentParticipations> AddSubscriptionAsync(Tournament tournament, User user)
    //{
    //    var inserted = await _dbCtx.Set<UserTournamentParticipations>()
    //        .AddAsync(new UserTournamentParticipations()
    //        {
    //            Tournament = tournament,
    //            User = user
    //        });
    //    await _dbCtx.SaveChangesAsync();
    //    return inserted.Entity;
    //}

    public async Task<int> GetSubscriberNumberAsync(long tournamentId)
    {
        //return await _dbCtx.Set<UserTournamentParticipations>()
        //    .Where(t => t.Tournament.Id == tournamentId)
        //    .CountAsync();
        throw new NotImplementedException();
    }

    public async Task<Tournament> GetTournamentByChallengeAsync(long challengeId)
    {
        var challenge = await _dbCtx.Challenges
            .Include(c => c.Tournament)
            .FirstAsync(c => c.Id == challengeId);

        return challenge.Tournament;
    }
}