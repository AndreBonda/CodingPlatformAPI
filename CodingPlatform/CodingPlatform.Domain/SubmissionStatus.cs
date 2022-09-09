namespace CodingPlatform.Domain;

public class SubmissionStatus
{
    public DateTime StartDate { get;}
    public DateTime EndDate { get; }
    public DateTime? SubmitDate { get;}
    public string Content { get; init; }
    public decimal Score { get; init; }
    public string[] TipsUsed { get; init; }
    public int TotalAvailableTips { get; init; }
    
    public SubmissionStatus(DateTime startDate, DateTime endDate, DateTime? submitDate = null)
    {
        if (startDate >= endDate)
            throw new ArgumentException("StartDate must be lower than EndDate");

        if (submitDate.HasValue && submitDate <= startDate)
            throw new ArgumentException("SubmitDate must be greater than startDate");

        if (submitDate.HasValue && submitDate > endDate)
            throw new ArgumentException("SubmitDate must be lower or equal than EndDate");

        StartDate = startDate;
        EndDate = endDate;
        SubmitDate = submitDate;
        TipsUsed = Array.Empty<string>();
    }

    public int TipsUsedNumber() => TipsUsed.Length;
    public int AvailableTips() => TotalAvailableTips - TipsUsed.Length;
}