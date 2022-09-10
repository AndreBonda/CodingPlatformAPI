using CodingPlatform.Domain.Entities;

namespace CodingPlatform.Domain;

public class SubmissionStatus
{
    public long SubmissionId { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public DateTime? SubmitDate { get; private set; }
    public string ChallengeTitle { get; }
    public string ChallengeDescription { get; }
    public List<Tip> ChallengeTips { get; }
    public int UsedTips { get; private set; }
    public string Content { get; set; }
    public decimal Score { get; init; }

    public SubmissionStatus(Submission submission, Challenge challenge, IEnumerable<Tip> challengeTips)
    {
        SubmissionId = submission.Id;
        StartDate = submission.DateCreated;
        SubmitDate = submission.DateSubmitted;
        EndDate = challenge.EndDate;
        ChallengeTitle = challenge.Title;
        ChallengeDescription = challenge.Description;
        ChallengeTips = challengeTips.ToList();
        UsedTips = submission.TipsNumber;
    }

    public int ChallengeTipAvailableNumber() => ChallengeTips.Count;
    public int RemainingTipsNumber() => ChallengeTips.Count - UsedTips;
    public bool IsRemainingTip() => (ChallengeTips.Count - UsedTips) > 0;
    public bool IsSubmissionDelivered() => SubmitDate.HasValue;
    public bool IsChallengeOver() => DateTime.UtcNow > EndDate;
    public void AddTips()
    {
        if (RemainingTipsNumber() > 0)
            UsedTips += 1;
    }

    public void EndSubmission(string content)
    {
        if (!IsSubmissionDelivered() && !IsChallengeOver())
        {
            SubmitDate = DateTime.UtcNow;
            Content = content;
        }
    }

    public IEnumerable<string> GetUsedTips() => ChallengeTips
        .Where(t => t.Order <= UsedTips)
        .Select(t => t.Description);
}