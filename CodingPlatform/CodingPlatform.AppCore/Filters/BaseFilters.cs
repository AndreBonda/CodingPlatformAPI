namespace CodingPlatform.AppCore.Filters;

public class BaseFilters
{
    private int? _limit;
    private int? _page;

    public int Take
    {
        get => _limit ?? 50; 
        set => _limit = value;
    }

    public int Page
    {
        get => _page ?? 0;
        set => _page = value;
    }
}