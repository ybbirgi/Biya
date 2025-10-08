namespace Domain.Entities.Events;

[Serializable]
public class EntityChangedEventData<TEntity> : EntityEventData<TEntity>
{
    public EntityChangedEventData(TEntity entity)
        : base(entity)
    {

    }
}
