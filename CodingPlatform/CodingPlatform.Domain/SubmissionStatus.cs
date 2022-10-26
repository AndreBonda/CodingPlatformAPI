using CodingPlatform.Domain.Exception;

namespace CodingPlatform.Domain;

public class SubmissionStatus
{
    private const decimal _MAX_STARTING_SCORE = 5;
    private const decimal _MIN_STARTING_SCORE = 0;
    private const decimal _TIP_MALUS_PERCENTAGE = 0.1m;

    public long SubmissionId { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public DateTime? SubmitDate { get; private set; }
    public string ChallengeTitle { get; }
    public string ChallengeDescription { get; }
    public List<Tip> ChallengeTips { get; }
    public int UsedTips { get; private set; }
    public string Content { get; private set; }
    public decimal Score { get; private set; }

    //TODO: refactor and pass one parameter with required value
    public SubmissionStatus(Submission submission, Challenge challenge, IEnumerable<Tip> challengeTips)
    {
        SubmissionId = submission.Id;
        StartDate = submission.CreateDate;
        SubmitDate = submission.SubmitDate;
        EndDate = challenge.EndDate;
        ChallengeTitle = challenge.Title;
        ChallengeDescription = challenge.Description;
        ChallengeTips = challengeTips != null ? challengeTips.ToList() : new List<Tip>();
        UsedTips = submission.TipsNumber;
        Content = submission.Content;
        Score = submission.Score;
    }

    public int ChallengeTipAvailableNumber() => ChallengeTips.Count;
    public int RemainingTipsNumber() => ChallengeTips.Count - UsedTips;
    public bool IsRemainingTip() => RemainingTipsNumber() > 0;
    public bool IsSubmissionDelivered() => SubmitDate.HasValue;
    public bool IsChallengeOver() => DateTime.UtcNow > EndDate;

    public void AddTips()
    {
        if (IsSubmissionDelivered()) throw new BadRequestException("Submission is already delivered");
        if (IsChallengeOver()) throw new BadRequestException("Out of time: challenge is over");
        if (!IsRemainingTip()) throw new BadRequestException("No tips available");

        UsedTips += 1;
    }

    public void EndSubmission(string content)
    {
        if (IsSubmissionDelivered()) throw new BadRequestException("Submission is already delivered");
        if (IsChallengeOver()) throw new BadRequestException("Out of time: challenge is over");

        SubmitDate = DateTime.UtcNow;
        Content = content;
    }

    public void SetScore(int startingScore)
    {
        if (!IsSubmissionDelivered())
            throw new BadRequestException("Submission is not delivered");

        if (startingScore < _MIN_STARTING_SCORE || startingScore > _MAX_STARTING_SCORE)
            throw new ArgumentException($"Starting score must be between ${_MIN_STARTING_SCORE} and {_MAX_STARTING_SCORE}");

        decimal tipMalusValue = _MAX_STARTING_SCORE * _TIP_MALUS_PERCENTAGE;
        Score = Math.Max(0, startingScore - tipMalusValue * UsedTips); //avoid negative score
    }

    public IEnumerable<string> GetUsedTips() => ChallengeTips
        .Where(t => t.Order <= UsedTips)
        .OrderBy(t => t.Order)
        .Select(t => t.Description);
}