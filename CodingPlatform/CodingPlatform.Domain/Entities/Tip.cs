namespace CodingPlatform.Domain.Entities;

public class Tip : BaseEntity
{
    public string Description { get; set; }
    public byte Order { get; set; }
}