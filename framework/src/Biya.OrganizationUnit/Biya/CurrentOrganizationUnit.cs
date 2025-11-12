namespace Biya;

public class CurrentOrganizationUnit : ICurrentOrganizationUnit, ITransientDependency
{
    public virtual bool IsAvailable => Id.HasValue;

    public virtual Guid? Id => _currentOrganizationUnitAccessor.Current?.OrganizationUnitId;

    public string? Name => _currentOrganizationUnitAccessor.Current?.Name;

    private readonly ICurrentOrganizationUnitAccessor _currentOrganizationUnitAccessor;

    public CurrentOrganizationUnit(
        ICurrentOrganizationUnitAccessor currentOrganizationUnitAccessor
    )
    {
        _currentOrganizationUnitAccessor = currentOrganizationUnitAccessor;
    }

    public IDisposable Change(
        Guid? id,
        string? name = null
    )
    {
        return SetCurrent(id, name);
    }

    private IDisposable SetCurrent(
        Guid? tenantId,
        string? name = null
    )
    {
        var parentScope = _currentOrganizationUnitAccessor.Current;
        _currentOrganizationUnitAccessor.Current = new BasicOrganizationUnitInfo(tenantId, name);

        return new DisposeAction<ValueTuple<ICurrentOrganizationUnitAccessor, BasicOrganizationUnitInfo?>>(static (
            state
        ) =>
        {
            var (currentOrganizationUnitAccessor, parentScope) = state;
            currentOrganizationUnitAccessor.Current = parentScope;
        }, (_currentOrganizationUnitAccessor, parentScope));
    }
}