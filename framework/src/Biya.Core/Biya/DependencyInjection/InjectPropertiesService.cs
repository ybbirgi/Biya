using System.Reflection;

namespace Biya.DependencyInjection;

public class InjectPropertiesService : IInjectPropertiesService, ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }

    public InjectPropertiesService(
        IServiceProvider serviceProvider
    )
    {
        ServiceProvider = serviceProvider;
    }

    public TService InjectProperties<TService>(
        TService instance
    ) where TService : notnull
    {
        return InjectPropertiesInternal(instance, true);
    }

    public TService InjectUnsetProperties<TService>(
        TService instance
    ) where TService : notnull
    {
        return InjectPropertiesInternal(instance, false);
    }

    protected virtual TService InjectPropertiesInternal<TService>(
        TService instance,
        bool overwrite
    ) where TService : notnull
    {
        var type = instance.GetType();

        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite && p.PropertyType.IsInterface)
            .ToList();

        foreach (var property in properties)
        {
            if (!overwrite)
            {
                if (property.GetValue(instance) != null)
                {
                    continue;
                }
            }

            var service = ServiceProvider.GetService(property.PropertyType);

            if (service == null)
            {
                continue;
            }

            property.SetValue(instance, service);
        }

        return instance;
    }
}