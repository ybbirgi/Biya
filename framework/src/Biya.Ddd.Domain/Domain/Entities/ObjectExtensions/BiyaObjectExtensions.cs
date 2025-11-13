using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Domain.Entities.ObjectExtensions;

public static class BiyaObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T As<T>(this object obj)
        where T : class
    {
        return (T)obj;
    }
    public static bool IsAssignableTo<TTarget>(this Type type)
    {
        return type.IsAssignableTo(typeof(TTarget));
    }

}