using System.Reflection;
using Biya.Modularity.ServiceLifetimes;
using Microsoft.Extensions.DependencyInjection;

namespace Biya.Modularity;

public static class DependencyRegistrar
{
    public static void RegisterAssembly(
        IServiceCollection services,
        Assembly assembly
    )
    {
        if (assembly == null) return;

        var types = assembly.GetTypes()
            .Where(t => t.IsClass && t is { IsAbstract: false, IsGenericTypeDefinition: false })
            .ToArray();

        foreach (var type in types)
        {
            if (typeof(BiyaModule).IsAssignableFrom(type)) continue;

            var interfaces = type.GetInterfaces();

            if (typeof(ITransientDependency).IsAssignableFrom(type))
            {
                Register(services, ServiceLifetime.Transient, type, interfaces);
                continue;
            }

            if (typeof(IScopedDependency).IsAssignableFrom(type))
            {
                Register(services, ServiceLifetime.Scoped, type, interfaces);
                continue;
            }

            if (typeof(ISingletonDependency).IsAssignableFrom(type))
            {
                Register(services, ServiceLifetime.Singleton, type, interfaces);
                continue;
            }
        }
    }

    private static void Register(
        IServiceCollection services,
        ServiceLifetime lifetime,
        Type implType,
        Type[] interfaces
    )
    {
        var serviceType = FindDefaultInterface(implType, interfaces) ?? implType;

        // Avoid duplicate registrations for the exact service/impl/lifetime combo
        if (!services.Any(sd =>
                sd.ServiceType == serviceType &&
                sd.ImplementationType == implType &&
                sd.Lifetime == lifetime))
        {
            var descriptor = new ServiceDescriptor(serviceType, implType, lifetime);
            services.Add(descriptor);
        }
    }

    private static Type? FindDefaultInterface(
        Type implType,
        Type[] interfaces
    )
    {
        var named = interfaces.FirstOrDefault(i => i.Name == $"I{implType.Name}");
        if (named != null) return named;

        // else return first non-marker interface (avoid our marker interfaces)
        var candidate = interfaces.FirstOrDefault(i =>
            i != typeof(ITransientDependency) &&
            i != typeof(IScopedDependency) &&
            i != typeof(ISingletonDependency));
        return candidate;
    }
}