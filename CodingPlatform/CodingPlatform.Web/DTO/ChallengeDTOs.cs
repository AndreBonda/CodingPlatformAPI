using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Web.DTO;

public class ChallengeDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IEnumerable<string> Tips { get; set; }
}

public class UserChallenges
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class CreateChallengeDto
{
    [Required][Range(1, long.MaxValue)] public long TournamentId { get; set; }

    [Required] public string Title { get; set; }

    [Required] public string Description { get; set; }

    [Required][Range(1, 3)] public int Hours { get; set; }

    public IEnumerable<string> Tips { get; set; }
}