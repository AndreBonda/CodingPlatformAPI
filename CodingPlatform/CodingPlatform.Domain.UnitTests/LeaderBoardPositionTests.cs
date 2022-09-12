using NUnit.Framework;

namespace CodingPlatform.Domain.UnitTests;

[TestFixture]
public class LeaderBoardPositionTests
{
    [TestCase(null)]
    [TestCase("")]
    public void LeaderBoardPosition_UsernameNullOrEmptyParam_ThrowArgumentNullException(string wrongUsername)
    {
        Assert.Throws<ArgumentNullException>(() => new LeaderBoardPosition(wrongUsername));
    }

    [Test]
    public void LeaderBoardPosition_CorrectUsernameParam_SetupUserNameProperty()
    {
        var lbp = new LeaderBoardPosition("username");

        Assert.That(lbp.UserName, Is.EqualTo("username"));
    }
    
    [Test]
    public void LeaderBoardPosition_StartingScore_DefaultValuePoints()
    {
        var lbp = new LeaderBoardPosition("username");

        Assert.That(lbp.TotalPoints, Is.EqualTo(0m));
        Assert.That(lbp.AveragePoints, Is.EqualTo(0m));
        Assert.That(lbp.SubmissionsNumber, Is.EqualTo(0));
    }
    
    [Test]
    public void AddScore_OneScore_CorrectPoints()
    {
        var lbp = new LeaderBoardPosition("username");
        
        lbp.AddScore(5);

        Assert.That(lbp.TotalPoints, Is.EqualTo(5m));
        Assert.That(lbp.AveragePoints, Is.EqualTo(5m));
        Assert.That(lbp.SubmissionsNumber, Is.EqualTo(1));
    }
    
    [Test]
    public void AddScore_TwoScores_CorrectPoints()
    {
        var lbp = new LeaderBoardPosition("username");
        
        lbp.AddScore(5);
        lbp.AddScore(4);

        Assert.That(lbp.TotalPoints, Is.EqualTo(9m));
        Assert.That(lbp.AveragePoints, Is.EqualTo(4.5m));
        Assert.That(lbp.SubmissionsNumber, Is.EqualTo(2));
    }
}