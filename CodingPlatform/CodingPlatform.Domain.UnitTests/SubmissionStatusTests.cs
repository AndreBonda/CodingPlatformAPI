using CodingPlatform.Domain.Exception;
using NUnit.Framework;

namespace CodingPlatform.Domain.UnitTests;

[TestFixture]
public class SubmissionStatusTests
{
    private Submission _submission;
    private Challenge _challenge;
    private List<Tip> _tips;

    [SetUp]
    public void SetUp()
    {
        _submission = new Submission();
        _challenge = new Challenge();
        _tips = new List<Tip>();
    }

    [Test]
    public void ChallengeTipAvailableNumber_TipsEmptyList_ReturnZero()
    {
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        Assert.That(subStatus.ChallengeTipAvailableNumber(), Is.EqualTo(0));
    }

    [Test]
    public void ChallengeTipAvailableNumber_TipsNull_ReturnZero()
    {
        var subStatus = new SubmissionStatus(_submission, _challenge, null);

        Assert.That(subStatus.ChallengeTipAvailableNumber(), Is.EqualTo(0));
    }

    [Test]
    public void ChallengeTipAvailableNumber_TipsAvailable_ReturnCorrectValue()
    {
        _tips.Add(new Tip());
        _tips.Add(new Tip());
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        Assert.That(subStatus.ChallengeTipAvailableNumber(), Is.EqualTo(2));
    }

    [Test]
    public void RemainingTipsNumber_NoRemainingTips_ReturnZero()
    {
        _tips.Add(new Tip());
        _tips.Add(new Tip());
        _submission.TipsNumber = 2;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        Assert.That(subStatus.RemainingTipsNumber(), Is.EqualTo(0));
    }

    [Test]
    public void RemainingTipsNumber_RemainingTips_ReturnCorrectValue()
    {
        _tips.Add(new Tip());
        _tips.Add(new Tip());
        _submission.TipsNumber = 1;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        Assert.That(subStatus.RemainingTipsNumber(), Is.EqualTo(1));
    }

    [Test]
    public void IsRemainingTip_NoRemainingTips_ReturnFalse()
    {
        _tips.Add(new Tip());
        _tips.Add(new Tip());
        _submission.TipsNumber = 2;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        Assert.That(subStatus.IsRemainingTip, Is.EqualTo(false));
    }

    [Test]
    public void IsRemainingTip_RemainingTips_ReturnTrue()
    {
        _tips.Add(new Tip());
        _tips.Add(new Tip());
        _submission.TipsNumber = 1;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        Assert.That(subStatus.IsRemainingTip, Is.EqualTo(true));
    }

    [Test]
    public void IsSubmissionDelivered_SubmissionNotDelivered_ReturnFalse()
    {
        _submission.DateSubmitted = null;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        Assert.That(subStatus.IsSubmissionDelivered, Is.EqualTo(false));
    }

    [Test]
    public void IsSubmissionDelivered_SubmissionDelivered_ReturnTrue()
    {
        _submission.DateSubmitted = DateTime.UtcNow;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        Assert.That(subStatus.IsSubmissionDelivered, Is.EqualTo(true));
    }

    [Test]
    public void IsChallengeOver_ChallengeIsOver_ReturnTrue()
    {
        _challenge.EndDate = DateTime.UtcNow;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        Assert.That(subStatus.IsChallengeOver, Is.EqualTo(true));
    }

    [Test]
    public void IsChallengeOver_ChallengeIsNotOver_ReturnFalse()
    {
        _challenge.EndDate = DateTime.UtcNow.AddDays(1);
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        Assert.That(subStatus.IsChallengeOver, Is.EqualTo(false));
    }

    [Test]
    public void AddTips_SubmissionIsDelivered_ThrowBadRequestException()
    {
        _submission.DateSubmitted = DateTime.UtcNow;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        var exc = Assert.Throws<BadRequestException>(() => subStatus.AddTips());
        Assert.That(exc.Message, Does.Contain("deliver").IgnoreCase);
    }

    [Test]
    public void AddTips_ChallengeIsOver_ThrowBadRequestException()
    {
        _challenge.EndDate = DateTime.UtcNow;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        var exc = Assert.Throws<BadRequestException>(() => subStatus.AddTips());
        Assert.That(exc.Message, Does.Contain("over").IgnoreCase);
    }

    [Test]
    public void AddTips_NoMoreTipsAvailable_ThrowBadRequestException()
    {
        _submission.DateSubmitted = null;
        _challenge.EndDate = DateTime.UtcNow.AddDays(1);
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        var exc = Assert.Throws<BadRequestException>(() => subStatus.AddTips());
        Assert.That(exc.Message, Does.Contain("available").IgnoreCase);
    }

    [Test]
    public void AddTips_WhenCalledCorrectly_SetUsedTips()
    {
        _submission.DateSubmitted = null;
        _challenge.EndDate = DateTime.UtcNow.AddDays(1);
        _tips.Add(new Tip());
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        subStatus.AddTips();

        Assert.That(subStatus.UsedTips, Is.EqualTo(1));
    }

    [Test]
    public void EndSubmission_SubmissionIsDelivered_ThrowBadRequestException()
    {
        _submission.DateSubmitted = DateTime.UtcNow;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        var exc = Assert.Throws<BadRequestException>(() => subStatus.EndSubmission(""));
        Assert.That(exc.Message, Does.Contain("deliver").IgnoreCase);
    }

    [Test]
    public void EndSubmission_ChallengeIsOver_ThrowBadRequestException()
    {
        _challenge.EndDate = DateTime.UtcNow;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        var exc = Assert.Throws<BadRequestException>(() => subStatus.EndSubmission(""));
        Assert.That(exc.Message, Does.Contain("over").IgnoreCase);
    }

    [Test]
    public void EndSubmission_WhenCalledCorrectly_SetContentAndSubmitDate()
    {
        _submission.DateSubmitted = null;
        _challenge.EndDate = DateTime.UtcNow.AddDays(1);
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        subStatus.EndSubmission("Content");

        Assert.That(subStatus.SubmitDate, Is.Not.Null);
        Assert.That(subStatus.Content, Is.EqualTo("Content"));
    }

    [Test]
    public void SetScore_SubmissionNotDelivered_ThrowBadRequestException()
    {
        _submission.DateSubmitted = null;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        var exc = Assert.Throws<BadRequestException>(() => subStatus.SetScore(1));
        Assert.That(exc.Message, Does.Contain("deliver").IgnoreCase);
    }

    [TestCase(-1)]
    [TestCase(6)]
    public void SetScore_StartingScoreOutOfRange_ThrowArgumentException(int startingScore)
    {
        _submission.DateSubmitted = DateTime.UtcNow;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        var exc = Assert.Throws<ArgumentException>(() => subStatus.SetScore(startingScore));
        Assert.That(exc.Message, Does.Contain("starting").IgnoreCase);
    }

    [TestCase(0, 0)]
    [TestCase(5, 5)]
    public void SetScore_NoTipsUsed_ScoreIsEqualToStartingScore(int startingScore, decimal expected)
    {
        _submission.DateSubmitted = DateTime.UtcNow;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        subStatus.SetScore(startingScore);

        Assert.That(subStatus.Score, Is.EqualTo(expected));
    }

    [TestCase(1, 0.5)]
    [TestCase(5, 4.5)]
    public void SetScore_OneTipsUsed_ScoreIsEqualToStartingScore(int startingScore, decimal expected)
    {
        _submission.DateSubmitted = DateTime.UtcNow;
        _submission.TipsNumber = 1;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        subStatus.SetScore(startingScore);

        Assert.That(subStatus.Score, Is.EqualTo(expected));
    }

    [Test]
    public void SetScore_NegativeScore_ScoreRoundedToZero()
    {
        int startingScore = 0;
        _submission.DateSubmitted = DateTime.UtcNow;
        _submission.TipsNumber = 1;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        subStatus.SetScore(startingScore);

        Assert.That(subStatus.Score, Is.EqualTo(0));
    }

    [Test]
    public void GetUsedTips_TipsEmpty_ReturnEmptyCollectionTips()
    {
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        var usedTips = subStatus.GetUsedTips();

        Assert.That(usedTips.Count(), Is.EqualTo(0));
    }

    [Test]
    public void GetUsedTips_TipsUsed_ReturnOnlyTipsUsed()
    {
        _tips.Add(new Tip() { Description = "Tip1", Order = 1 });
        _tips.Add(new Tip() { Description = "Tip2", Order = 2 });
        _tips.Add(new Tip() { Description = "Tip3", Order = 3 });
        _submission.TipsNumber = 2;
        var subStatus = new SubmissionStatus(_submission, _challenge, _tips);

        var usedTips = subStatus.GetUsedTips();

        Assert.That(usedTips.Count(), Is.EqualTo(2));
        Assert.That(usedTips.ElementAt(0), Is.EqualTo("Tip1"));
        Assert.That(usedTips.ElementAt(1), Is.EqualTo("Tip2"));
        Assert.That(usedTips, Does.Not.Contain("Tip3"));
    }

}