using CodingPlatform.Domain;
using Moq;
using NUnit.Framework;

namespace CodingPlatform.AppCore.UnitTests;

[TestFixture]
public class TournamentInfoTests
{
    [TestCase(2,0,2)]
    [TestCase(3,1,2)]
    [TestCase(3,3,0)]
    public void GetAvailableSeats_DefaultBehavior_ReturnZero(int maxParticipants, int subscribeNumber, int expected)
    {
        var tournamentInfo = new TournamentInfo(1,"name", maxParticipants, "admin", subscribeNumber, DateTime.Now);
        
        Assert.That(tournamentInfo.GetAvailableSeats(), Is.EqualTo(expected));
    }
}