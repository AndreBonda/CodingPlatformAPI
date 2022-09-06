using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface ITournamentRepository : IRepository<Tournament>
{
    Task<IEnumerable<Tournament>> GetFiltered(TournamentFilters f);
    Task<Tournament> GetTournamentByName(string name);
}