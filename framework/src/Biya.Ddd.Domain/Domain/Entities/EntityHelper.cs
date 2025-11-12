using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Biya;
using Biya.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Some helper methods for entities.
/// </summary>
public static class EntityHelper
{
    public static bool IsMultiTenant<TEntity>()
        where TEntity : IEntity
    {
        return IsMultiTenant(typeof(TEntity));
    }

    public static bool IsMultiTenant(
        Type type
    )
    {
        return typeof(IMultiTenant).IsAssignableFrom(type);
    }
}