using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Biya.DependencyInjection;

public static class BiyaDependencyInjectionExtensions
{
    private static readonly Type[] DependencyInterfaces = new[]
    {
        typeof(ITransientDependency),
        typeof(IScopedDependency),
        typeof(ISingletonDependency)
    };

    /// <summary>
    /// Verilen başlangıç assembly'sinden başlayarak tüm proje referanslarını tarar ve
    /// DI marker arayüzlerini (ITransientDependency, IScopedDependency, ISingletonDependency) uygulayan 
    /// sınıfları otomatik olarak DI konteynerine kaydeder.
    /// Kural: Somut sınıf, var olan ilk arayüzü ile eşleştirilir. Arayüz yoksa, somut tip ile kaydedilir.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="startingAssembly">Tarama işlemine başlanacak ana assembly.</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AutoRegisterDependencies(
        this IServiceCollection services,
        Assembly startingAssembly
    )
    {
        var assembliesToScan = GetReferencedAssemblies(startingAssembly);

        assembliesToScan.Add(startingAssembly);

        var concreteTypes = assembliesToScan
            .SelectMany(a =>
            {
                try
                {
                    return a.GetTypes();
                }
                catch (ReflectionTypeLoadException)
                {
                    return Array.Empty<Type>();
                }
            })
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition)
            .ToList();

        foreach (var type in concreteTypes)
        {
            var lifetimeInterface = type.GetInterfaces()
                .FirstOrDefault(i => DependencyInterfaces.Contains(i));

            if (lifetimeInterface == null)
            {
                continue;
            }

            var serviceInterface = type.GetInterfaces()
                .Except(DependencyInterfaces)
                .FirstOrDefault(i => i.Name == "I" + type.Name);

            var serviceType = serviceInterface ?? type;
            var lifetime = GetServiceLifetime(lifetimeInterface);

            services.Add(new ServiceDescriptor(serviceType, type, lifetime));
        }

        return services;
    }

    /// <summary>
    /// Ana uygulama assembly'sini (Entry Assembly) kullanarak Biya framework assembly'lerini otomatik olarak kaydeder.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AutoRegisterBiyaFrameworkDependencies(
        this IServiceCollection services
    )
    {
        var entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly == null)
        {
            throw new InvalidOperationException(
                "Ana uygulama assembly'si (Entry Assembly) bulunamadı. Lütfen 'AutoRegisterDependencies' metodunu kullanarak başlangıç assembly'sini manuel olarak belirtin.");
        }

        return services.AutoRegisterDependencies(entryAssembly);
    }

    private static HashSet<Assembly> GetReferencedAssemblies(
        Assembly assembly
    )
    {
        var assemblies = new HashSet<Assembly>();
        var stack = new Stack<Assembly>();
        stack.Push(assembly);

        while (stack.Count > 0)
        {
            var currentAssembly = stack.Pop();
            foreach (var reference in currentAssembly.GetReferencedAssemblies())
            {
                try
                {
                    var referencedAssembly = Assembly.Load(reference);

                    if (referencedAssembly.FullName != null &&
                        (referencedAssembly.FullName.StartsWith("Biya.") ||
                         referencedAssembly == Assembly.GetEntryAssembly()) &&
                        !assemblies.Contains(referencedAssembly))
                    {
                        assemblies.Add(referencedAssembly);
                        stack.Push(referencedAssembly);
                    }
                }
                catch (FileNotFoundException)
                {
                    // Referans bulunamazsa devam et
                }
            }
        }

        return assemblies;
    }

    private static ServiceLifetime GetServiceLifetime(
        Type lifetimeInterface
    )
    {
        if (lifetimeInterface == typeof(ITransientDependency))
        {
            return ServiceLifetime.Transient;
        }

        if (lifetimeInterface == typeof(IScopedDependency))
        {
            return ServiceLifetime.Scoped;
        }

        if (lifetimeInterface == typeof(ISingletonDependency))
        {
            return ServiceLifetime.Singleton;
        }

        throw new ArgumentException($"Bilinmeyen bağımlılık arayüzü: {lifetimeInterface.Name}");
    }
}