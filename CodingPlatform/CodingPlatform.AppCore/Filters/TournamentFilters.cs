namespace CodingPlatform.AppCore.Filters;

public class TournamentSearch : BaseSearch
{
    public string TournamentName { get; set; }

    public TournamentSearch(int? take = null) : base(take)
    {
    }
}