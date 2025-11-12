namespace Biya;

public interface ICurrentTenantAccessor
{
    BasicTenantInfo? Current { get; set; }
}