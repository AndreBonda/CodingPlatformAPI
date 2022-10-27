namespace CodingPlatform.Web.DTO;

public class SubmissionDto
{
    public long SubmissionId { get; set; }
    public DateTime Started { get; set; }
    public DateTime? Submitted { get; set; }
    public string Content { get; set; }
    public decimal Score { get; set; }
}

public class SubmissionStatusDto : SubmissionDto
{
    public DateTime EndDate { get; set; }
    public string ChallengeTitle { get; set; }
    public string ChallengeDescription { get; set; }
    public string[] UsedTips { get; set; }
    public int ChallengeTipAvailableNumber { get; set; }
    public int RemainingTipsNumber { get; set; }
}

public class SearchSubmissionDTO
{
    public bool OnlySubmitted { get; set; }
    public bool ExcludeEvaluated { get; set; }
}