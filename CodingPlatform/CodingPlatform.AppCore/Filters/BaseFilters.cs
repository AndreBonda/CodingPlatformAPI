namespace CodingPlatform.AppCore.Filters;

public abstract class BaseFilters
{
    public int Take { get; }

    protected BaseFilters(int? take = null)
    {
        Take = take ?? 50;
    }
}