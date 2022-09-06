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
}