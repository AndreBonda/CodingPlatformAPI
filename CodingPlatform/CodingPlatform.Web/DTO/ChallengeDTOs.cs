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

public class InfoInProgressChallengeDto
{
    public long ChallengeId { get; set; }
    public string ChallengeName { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string TournamentName { get; set; }
}

public class SubmissionStatusDto
{
    public DateTime Started { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? Submitted { get; set; }
    public string Content { get; set; }
    public decimal Score { get; set; }
    public string[] TipsUsed { get; set; }
    public int TotalAvailableTips { get; set; }
    public int TipsUsedNumber { get; set; }
    public int AvailableTips { get; set; }

}

public class CreateChallengeDto
{
    [Required] [Range(0, long.MaxValue)] public long TournamentId { get; set; }

    [Required] public string Title { get; set; }

    [Required] public string Description { get; set; }

    [Required] [Range(1, 3)] public int Hours { get; set; }

    public IEnumerable<string> Tips { get; set; }
}