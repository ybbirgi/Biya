namespace Biya.Exceptions;

public class BiyaException : Exception
{
    public BiyaException()
    {
    }

    public BiyaException(
        string? message
    )
        : base(message)
    {
    }

    public BiyaException(
        string? message,
        Exception? innerException
    )
        : base(message, innerException)
    {
    }
}