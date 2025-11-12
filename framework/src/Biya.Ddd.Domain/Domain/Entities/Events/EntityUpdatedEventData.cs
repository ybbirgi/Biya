namespace Domain.Entities.Events;

[Serializable]
public class EntityUpdatedEventData<TEntity> : EntityChangedEventData<TEntity>
{
    public EntityUpdatedEventData(TEntity entity)
        : base(entity)
    {

    }
}
