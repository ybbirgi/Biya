using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Domain.Entities.ObjectExtensions;

namespace Domain.Entities;

[Serializable]
public abstract class AggregateRoot :
    Entity,
    IHasExtraProperties,
    IHasConcurrencyStamp
{
    public virtual JsonDocument ExtraProperties { get; protected set; }
    public virtual string ConcurrencyStamp { get; set; }

    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = JsonDocument.Parse("{}");
    }
}

[Serializable]
public abstract class AggregateRoot<TKey> : Entity<TKey>,
    IHasExtraProperties,
    IHasConcurrencyStamp
{
    public virtual JsonDocument ExtraProperties { get; protected set; }

    public virtual string ConcurrencyStamp { get; set; }

    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = JsonDocument.Parse("{}");
    }

    protected AggregateRoot(
        TKey id
    )
        : base(id)
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = JsonDocument.Parse("{}");
    }
}