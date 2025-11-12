namespace Biya;

public class AsyncLocalCurrentOrganizationUnitAccessor : ICurrentTenantAccessor
{
    public static AsyncLocalCurrentOrganizationUnitAccessor Instance { get; } = new();

    public BasicTenantInfo? Current {
        get => _currentScope.Value;
        set => _currentScope.Value = value;
    }

    private readonly AsyncLocal<BasicTenantInfo?> _currentScope;

    private AsyncLocalCurrentOrganizationUnitAccessor()
    {
        _currentScope = new AsyncLocal<BasicTenantInfo?>();
    }
}
