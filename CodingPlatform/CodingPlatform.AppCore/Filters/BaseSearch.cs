namespace CodingPlatform.AppCore.Filters;

public abstract class BaseSearch
{
    public int Take { get; }

    protected BaseSearch(int? take = null)
    {
        Take = take ?? 50;
    }
}