using CodingPlatform.Domain.Entities;
using CodingPlatform.Domain.Extensions;
using NUnit.Framework;

namespace CodingPlatform.Domain.UnitTests;

[TestFixture]
public class ChallengeExtensionsTests
{
    [Test]
    public void IsInProgress_ChallengeIsNotInProgress_ReturnFalse()
    {
        var challenge = new Challenge
        {
            DateCreated = DateTime.UtcNow.AddDays(-2),
            EndDate = DateTime.UtcNow.AddDays(-1)
        };
        
        Assert.That(challenge.IsInProgress(), Is.EqualTo(false));
    }
    
    [Test]
    public void IsInProgress_ChallengeIsInProgress_ReturnTrue()
    {
        var challenge = new Challenge
        {
            DateCreated = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(1)
        };
        
        Assert.That(challenge.IsInProgress(), Is.EqualTo(true));
    }
}