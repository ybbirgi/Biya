using Microsoft.Extensions.Logging;

namespace Biya.Logging;

public interface IHasLogLevel
{
    LogLevel LogLevel { get; set; }
}