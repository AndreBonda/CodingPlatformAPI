using System.ComponentModel.DataAnnotations;
using CodingPlatform.Domain.Exception;

namespace CodingPlatform.Domain;

public class Submission : BaseEntity
{
    private const decimal _MAX_STARTING_SCORE = 5;
    private const decimal _MIN_STARTING_SCORE = 0;
    private const decimal _TIP_MALUS_PERCENTAGE = 0.1m;

    public byte TipsNumber { get; private set; }
    public DateTime? SubmitDate { get; private set; }
    public string Content { get; private set; }
    public decimal Score { get; private set; }
    public DateTime? EvaluateDate { get; set; }
    [Required]
    public User User { get; private set; }
    [Required]
    public User Admin { get; private set; }
    [Required]
    public Challenge Challenge { get; private set; }

    public int RemainingTips() => Challenge.TotalTips() - TipsNumber;

    public IEnumerable<Tip> GetAvailableTips()
    {
        foreach (var tip in Challenge.Tips.OrderBy(t => t.Order))
        {
            if (tip.Order > TipsNumber) yield break;
            yield return tip;
        }
    }

    public Tip GetLastAvailableTips() => GetAvailableTips().Last();

    public void RequestNewTip(long userId)
    {
        VerifyUser(userId);

        if (IsSubmitted()) throw new BadRequestException("Challenge already submitted");

        if (!Challenge.IsActive()) throw new BadRequestException("Challenge is not active");

        if (RemainingTips() == 0)
            throw new BadRequestException("No more tips are available");

        TipsNumber++;
    }

    public void EndSubmission(string content, long userId)
    {
        VerifyUser(userId);

        if (IsSubmitted()) throw new BadRequestException("Challenge already submitted");

        if (!Challenge.IsActive()) throw new BadRequestException("Challenge is not active");

        SubmitDate = DateTime.UtcNow;
        Content = content;
    }

    public bool IsSubmitted() => SubmitDate.HasValue;

    public bool IsEvaluated() => EvaluateDate.HasValue;

    public void Evaluate(int score, long userId)
    {
        VerifyAdmin(userId);

        if (!IsSubmitted()) throw new BadRequestException("You can evaluate only submitted submissioms");

        EvaluateDate = DateTime.UtcNow;
        Score = CalculateFinalScore(score);
    }

    //TODO: spostare in una classe ad hoc?
    /// <summary>
    /// Calculates the final score starting from the initial score passed as a parameter.
    /// </summary>
    /// <param name="initialScore">Initial score assigned by admin</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public decimal CalculateFinalScore(int initialScore)
    {
        if (initialScore < _MIN_STARTING_SCORE || initialScore > _MAX_STARTING_SCORE)
            throw new ArgumentOutOfRangeException(nameof(initialScore));

        decimal tipMalusValue = _MAX_STARTING_SCORE * _TIP_MALUS_PERCENTAGE;

        // Use Max to avoid negative final-score
        return Math.Max(0, initialScore - tipMalusValue * TipsNumber);
    }

    private void VerifyUser(long userId)
    {
        if (userId != User.Id) throw new ForbiddenException("User not allowed to this submission");
    }

    private void VerifyAdmin(long userId)
    {
        if (userId != Admin.Id) throw new ForbiddenException("Only the admin can evaluate this submission");
    }

    public static Submission CreateNew(User user, User admin, Challenge challenge)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (admin == null) throw new ArgumentNullException(nameof(admin));

        if (challenge == null) throw new ArgumentNullException(nameof(challenge));

        return new Submission
        {
            TipsNumber = 0,
            Content = String.Empty,
            Score = 0,
            User = user,
            Admin = admin,
            Challenge = challenge
        };
    }
}