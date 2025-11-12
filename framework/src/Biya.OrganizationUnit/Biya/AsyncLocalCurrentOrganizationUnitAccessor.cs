namespace Biya;

public class AsyncLocalCurrentOrganizationUnitAccessor : ICurrentOrganizationUnitAccessor
{
    public static AsyncLocalCurrentOrganizationUnitAccessor Instance { get; } = new();

    public BasicOrganizationUnitInfo? Current {
        get => _currentScope.Value;
        set => _currentScope.Value = value;
    }

    private readonly AsyncLocal<BasicOrganizationUnitInfo?> _currentScope;

    private AsyncLocalCurrentOrganizationUnitAccessor()
    {
        _currentScope = new AsyncLocal<BasicOrganizationUnitInfo?>();
    }
}
