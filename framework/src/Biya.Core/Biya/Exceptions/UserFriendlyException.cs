using Microsoft.Extensions.Logging;

namespace Biya.Exceptions;

public class UserFriendlyException : BusinessException, IUserFriendlyException
{
    public UserFriendlyException(
        string message,
        string? code = null,
        string? details = null,
        Exception? innerException = null,
        LogLevel logLevel = LogLevel.Warning)
        : base(
              code,
              message,
              details,
              innerException,
              logLevel)
    {
        Details = details;
    }
}
