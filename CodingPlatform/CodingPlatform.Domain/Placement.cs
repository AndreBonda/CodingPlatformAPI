namespace CodingPlatform.Domain;

public class Placement
{
    public string Username { get; private set; }
    public int TotalChallenges { get; private set; }
    public decimal TotalPoints { get; private set; }
    public decimal AveragePoints { get; private set; }

    public Placement(string username, int totalChallenges, decimal totalPoints, decimal averagePoints)
    {
        Username = username;
        TotalChallenges = totalChallenges;
        TotalPoints = totalPoints;
        AveragePoints = averagePoints;
    }
}
