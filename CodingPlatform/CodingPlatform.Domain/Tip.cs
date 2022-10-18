namespace CodingPlatform.Domain;

public class Tip : BaseEntity
{
    public string Description { get; private set; }
    public byte Order { get; private set; }

    public Tip(string description, byte order)
    {
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException(nameof(description));
        if (order <= 0) throw new ArgumentException(nameof(order));

        Description = description;
        Order = order;
    }


}