namespace CodingPlatform.AppCore.Commands;

public class CreateChallengeCmd
{
    public long TournamentId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Hours { get; set; }
    public long UserId { get; set; }
    public IEnumerable<string> Tips { get; set; }
}

