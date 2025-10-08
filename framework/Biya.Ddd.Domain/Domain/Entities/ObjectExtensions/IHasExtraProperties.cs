using System.Text.Json;

namespace Domain.Entities.ObjectExtensions;

public interface IHasExtraProperties
{
    JsonDocument ExtraProperties { get; }
}