namespace CodingPlatform.Domain.Exception;

[Serializable]
public class BadRequestException : System.Exception
{
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, System.Exception inner) : base(message, inner) {}
}