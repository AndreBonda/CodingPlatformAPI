namespace CodingPlatform.Domain;

public class  LeaderBoardPosition
{
    public string UserName { get;}
    public decimal TotalPoints { get; private set; }
    public decimal AveragePoints { get; private set; }
    public int SubmissionsNumber { get; private set; }
    public int Place { get; set; }

    public LeaderBoardPosition(string userName)
    {
        if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException();

        UserName = userName;
    }

    public void AddScore(decimal score)
    {
        SubmissionsNumber += 1;
        TotalPoints += score;
        AveragePoints = TotalPoints / SubmissionsNumber;
    }
}