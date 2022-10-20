using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Web.DTO;

public class TournamentDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int MaxParticipants { get; set; }
    public DateTime DateCreated { get; set; }
    public string UsernameAdmin { get; set; }
    public int SubscriberNumber { get; set; }
    public int AvailableSeats { get; set; }
}

public class SearchTournamentDto : BaseSearchDto
{
    public string TournamentName { get; set; }
}

public class CreateTournamentDto
{
    [Required]
    public string TournamentName { get; set; }
    [Required]
    [Range(2, int.MaxValue)]
    public int MaxParticipants { get; set; }
}

public class LeaderBoardPositionDto
{
    public int Place { get; set; }
    public string UserName { get; set; }
    public decimal TotalPoints { get; set; }
    public decimal AveragePoints { get; set; }
}