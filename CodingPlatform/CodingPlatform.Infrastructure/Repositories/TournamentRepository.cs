using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public class TournamentRepository : BaseRepository<Tournament>, ITournamentRepository
{
    public TournamentRepository(AppDbContext dbCtx) : base(dbCtx)
    {
    }
    
    public async Task<IEnumerable<Tournament>>GetFiltered(TournamentFilters f = null)
    {
        f ??= new TournamentFilters();
        IQueryable<Tournament> results = dbCtx.Set<Tournament>();

        if (!string.IsNullOrWhiteSpace(f.TournamentName))
            results = results.Where(t => t.Name.ToLower().Contains(f.TournamentName.ToLower()));

        results = SetPagination(results, f);

        return await results.ToListAsync();
    }

    public async Task<Tournament> GetTournamentByName(string name)
    {
        if (name == null || string.IsNullOrEmpty(name)) return null;

        return await dbCtx.Set<Tournament>()
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
    }

    public async Task<bool> IsUserSubscribed(long tournamentId, long userId)
    {
        return await dbCtx.Set<UserTournamentParticipations>()
            .AnyAsync(utp => utp.Tournament.Id == tournamentId && utp.User.Id == userId);
    }

    public async Task<User> GetTournamentAdmin(long tournamentId)
    {
        var tournament = await dbCtx.Set<Tournament>()
            .Include(t => t.Admin)
            .FirstOrDefaultAsync(x => x.Id == tournamentId);
        
        return tournament?.Admin;
    }

    public async Task<UserTournamentParticipations> AddSubscription(Tournament tournament, User user)
    {
        var inserted = await dbCtx.Set<UserTournamentParticipations>()
            .AddAsync(new UserTournamentParticipations()
            {
                Tournament = tournament,
                User = user,
                DateCreated = DateTime.UtcNow
            });
        await dbCtx.SaveChangesAsync();
        return inserted.Entity;
    }

    public async Task<int> GetSubscriberNumber(long tournamentId)
    {
        return await dbCtx.Set<UserTournamentParticipations>()
            .Where(t => t.Tournament.Id == tournamentId)
            .CountAsync();
    }
}