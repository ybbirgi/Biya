namespace Domain.Entities.Events;

[Serializable]
public class EntityCreatedEventData<TEntity> : EntityChangedEventData<TEntity>
{
    public EntityCreatedEventData(TEntity entity)
        : base(entity)
    {

    }
}
