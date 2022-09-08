namespace CodingPlatform.Domain.Exception;

[Serializable]
public class ForbiddenException : System.Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, System.Exception inner) : base(message, inner)
    {
    }
}