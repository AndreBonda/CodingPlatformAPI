namespace CodingPlatform.Domain;

public class SubmissionStatus
{
    public DateTime StartDate { get;}
    public DateTime EndDate { get; }
    public DateTime? SubmitDate { get;}
    public List<string> TipsUsed { get; }
    public int TotalAvailableTips { get; }
    public string Content { get; init; }
    public decimal Score { get; init; }
    
    public SubmissionStatus(DateTime startDate, DateTime endDate, DateTime? submitDate = null, 
        int totalAvailableTips = 0, List<string> tipsUsed = null)
    {
        if (startDate >= endDate)
            throw new ArgumentException("StartDate must be lower than EndDate");

        if (submitDate.HasValue && submitDate <= startDate)
            throw new ArgumentException("SubmitDate must be greater than startDate");

        if (submitDate.HasValue && submitDate > endDate)
            throw new ArgumentException("SubmitDate must be lower or equal than EndDate");

        if (tipsUsed != null && tipsUsed.Count > totalAvailableTips)
            throw new ArgumentException("Tips used length must be lower or equal than TotalAvailableTips");

        StartDate = startDate;
        EndDate = endDate;
        SubmitDate = submitDate;
        TotalAvailableTips = totalAvailableTips;
        TipsUsed = tipsUsed ?? new List<string>();
    }

    public int TipsUsedNumber() => TipsUsed.Count;
    public int RemainingTips() => TotalAvailableTips - TipsUsed.Count;

    public bool AddTips(string tip)
    {
        if (tip == null) throw new ArgumentException();
        if (RemainingTips() == 0) return false;
        TipsUsed.Add(tip);
        return true;
    }

}