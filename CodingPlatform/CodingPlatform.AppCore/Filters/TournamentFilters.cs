namespace CodingPlatform.AppCore.Filters;

public class TournamentFilters : BaseFilters
{
    public string TournamentName { get; set; }

    public TournamentFilters(int? take = null) : base(take)
    {
    }
}