using System.Reflection;

namespace Biya.Modularity;

public static class ModuleLoader
{
    public static IEnumerable<Assembly> GetLoadedAssemblies()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic);
    }

    public static IEnumerable<Assembly> LoadAssembliesFromPath(
        string path,
        string searchPattern = "*.dll"
    )
    {
        var list = new List<Assembly>();
        if (!Directory.Exists(path)) return list;

        var dlls = Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
        foreach (var dll in dlls)
        {
            try
            {
                var name = AssemblyName.GetAssemblyName(dll);
                var already = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetName().FullName == name.FullName);
                if (already != null)
                {
                    list.Add(already);
                }
                else
                {
                    var asm = Assembly.LoadFrom(dll);
                    list.Add(asm);
                }
            }
            catch
            {
                // TODO :: Add logging
            }
        }

        return list;
    }

    public static IEnumerable<Assembly> CollectAssemblies(
        string? pluginFolder = null,
        IEnumerable<Assembly>? additionalAssemblies = null
    )
    {
        var set = new HashSet<Assembly>(GetLoadedAssemblies());

        if (additionalAssemblies != null)
        {
            foreach (var a in additionalAssemblies) set.Add(a);
        }

        if (!string.IsNullOrWhiteSpace(pluginFolder))
        {
            var fromPath = LoadAssembliesFromPath(pluginFolder);
            foreach (var a in fromPath) set.Add(a);
        }

        return set;
    }
}
