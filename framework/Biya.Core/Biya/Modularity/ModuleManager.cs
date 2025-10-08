using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Biya.Modularity;

public class ModuleManager
{
    private readonly List<BiyaModule> _moduleInstances = new();
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly IConfiguration _configuration;
    private readonly List<Assembly> _scannedAssemblies = new();

    public ModuleManager(
        IConfiguration configuration
    )
    {
        _configuration = configuration;
    }

    public void Initialize<TStartupModule>(
        string? pluginFolder = null,
        IEnumerable<Assembly>? additionalAssemblies = null
    )
        where TStartupModule : BiyaModule
    {
        var assemblies = ModuleLoader.CollectAssemblies(pluginFolder, additionalAssemblies).ToList();
        _scannedAssemblies.AddRange(assemblies);

        var moduleTypes = CollectModuleTypes(typeof(TStartupModule), assemblies);

        var sorted = TopologicalSortModules(moduleTypes);

        foreach (var t in sorted)
        {
            var instance = (BiyaModule)Activator.CreateInstance(t)!;
            _moduleInstances.Add(instance);
        }

        var context = new ServiceConfigurationContext(_services, _configuration);

        foreach (var module in _moduleInstances)
        {
            module.PreConfigureServices(context);

            DependencyRegistrar.RegisterAssembly(_services, module.GetType().Assembly);

            module.ConfigureServices(context);
        }

        foreach (var module in _moduleInstances)
            module.PostConfigureServices(context);
    }

    public IServiceProvider BuildServiceProvider()
    {
        var provider = _services.BuildServiceProvider();

        foreach (var module in _moduleInstances)
            module.OnApplicationInitialization(provider);

        return provider;
    }

    private static HashSet<Type> CollectModuleTypes(
        Type startupModule,
        IEnumerable<Assembly> assemblies
    )
    {
        var allTypes = new HashSet<Type>();
        var moduleByName = assemblies
            .SelectMany(a =>
            {
                try
                {
                    return a.GetTypes();
                }
                catch
                {
                    return Array.Empty<Type>();
                }
            })
            .Where(t => typeof(BiyaModule).IsAssignableFrom(t) && !t.IsAbstract && !t.IsGenericType)
            .ToDictionary(t => t.FullName!, t => t);

        void Collect(
            Type type
        )
        {
            if (allTypes.Contains(type)) return;

            allTypes.Add(type);

            var dependsAttrs = type.GetCustomAttributes<DependsOnAttribute>();
            foreach (var attr in dependsAttrs)
            {
                foreach (var dep in attr.DependedTypes)
                {
                    if (moduleByName.TryGetValue(dep.FullName, out var found))
                    {
                        Collect(found);
                    }
                    else
                    {
                        if (dep.IsSubclassOf(typeof(BiyaModule)))
                            Collect(dep);
                    }
                }
            }
        }

        if (moduleByName.TryGetValue(startupModule.FullName, out var startupFromAssemblies))
            Collect(startupFromAssemblies);
        else
            Collect(startupModule);

        return allTypes;
    }

    /// </summary>
    private static List<Type> TopologicalSortModules(
        HashSet<Type> modules
    )
    {
        var result = new List<Type>();
        var visited = new Dictionary<Type, bool>(); // false = visiting, true = visited

        void Visit(
            Type m
        )
        {
            if (visited.TryGetValue(m, out var done))
            {
                if (!done) throw new Exception($"Circular dependency detected on module {m.FullName}");
                return;
            }

            visited[m] = false;

            var depends = m.GetCustomAttributes<DependsOnAttribute>()
                .SelectMany(a => a.DependedTypes)
                .Where(t => typeof(BiyaModule).IsAssignableFrom(t));

            foreach (var d in depends)
            {
                var matched = modules.FirstOrDefault(x => x == d || x.FullName == d.FullName);
                if (matched != null) Visit(matched);
            }

            visited[m] = true;
            result.Add(m);
        }

        foreach (var m in modules)
            if (!visited.ContainsKey(m))
                Visit(m);

        return result;
    }
}
