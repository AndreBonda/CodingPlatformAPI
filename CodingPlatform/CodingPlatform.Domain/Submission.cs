using System.ComponentModel.DataAnnotations;
using CodingPlatform.Domain.Exception;

namespace CodingPlatform.Domain;

public class Submission : BaseEntity
{
    public byte TipsNumber { get; private set; }
    public DateTime? SubmitDate { get; private set; }
    public string Content { get; private set; }
    public decimal Score { get; private set; }
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

    private void VerifyUser(long userId)
    {
        if (userId != User.Id) throw new ForbiddenException("User not allowed to this submission");
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