namespace Domain.Entities.Events;

[Serializable]
public class EntityDeletedEventData<TEntity> : EntityChangedEventData<TEntity>
{
    public EntityDeletedEventData(TEntity entity)
        : base(entity)
    {

    }
}
