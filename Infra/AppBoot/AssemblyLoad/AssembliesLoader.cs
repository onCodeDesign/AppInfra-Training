using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace AppBoot.AssemblyLoad;

internal class AssembliesLoader(AssemblyLoadOptions options, IPluginPathBuilder pathBuilder, Func<Assembly[]> domainAssembliesProvider)
    : IAssembliesLoader
{
    private readonly HashSet<string> defaultAssemblies = new();
    private readonly AssemblyLoadContext defaultLoadContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly()) ?? AssemblyLoadContext.Default;

    public AssembliesLoader(AssemblyLoadOptions options, IPluginPathBuilder pathBuilder)
        : this(options, pathBuilder, AppDomain.CurrentDomain.GetAssemblies)
    {
    }

    public IEnumerable<Assembly> LoadAssemblies()
    {
        foreach (var assembly in LoadDomainAssemblies(domainAssembliesProvider()))
            yield return assembly;

        foreach (var assembly in LoadPluginAssemblies())
            yield return assembly;
    }

    private IEnumerable<Assembly> LoadDomainAssemblies(Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var assemblyName = assembly.GetName();
            if (assemblyName.Name != null && MatchesNameFilter(assemblyName.Name))
            {
                yield return assembly;
                foreach (var matchedDefaultAssembly in LoadDefaultAssemblies(assemblyName))
                {
                    yield return matchedDefaultAssembly;
                }
            }
        }
    }

    private IEnumerable<Assembly> LoadDefaultAssemblies(AssemblyName assemblyName)
    {
        var names = new Queue<AssemblyName>();
        names.Enqueue(assemblyName);
        while (names.TryDequeue(out var name))
        {
            if (name.Name == null || !defaultAssemblies.Add(name.Name))
                continue;

            // Load and find all dependencies of default assemblies.
            // This sacrifices some performance for determinism in how transitive
            // dependencies will be shared between host and plugins.
            var assembly = defaultLoadContext.LoadFromAssemblyName(name);
            //Console.WriteLine($"Default Assembly: {name.FullName}");

            foreach (var reference in assembly.GetReferencedAssemblies())
            {
                names.Enqueue(reference);
                if (reference.Name != null && MatchesNameFilter(reference.Name))
                {
                    var referencedAssembly = defaultLoadContext.LoadFromAssemblyName(reference);
                    yield return referencedAssembly;
                }
            }
        }
    }

    private IEnumerable<Assembly> LoadPluginAssemblies()
    {
        foreach (var plugin in options.Plugins.Where(MatchesNameFilter))
        {
            var pluginDescriptor = PluginDescriptor.Create(plugin, pathBuilder);
            PluginLoadContext loadContext = new PluginLoadContext(pluginDescriptor.Name, pluginDescriptor.GetAllPaths().ToArray(), defaultAssemblies);

            var pluginAssembly = loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginDescriptor.FullPath));
            var pluginAssemblyName = pluginAssembly.GetName().Name;
            if (pluginAssemblyName == null)
                continue;

            yield return pluginAssembly;

            HashSet<string> pluginLoadedAssemblies = new(defaultAssemblies);
            pluginLoadedAssemblies.Add(pluginAssemblyName);
            
            foreach (var pluginReference in LoadReferencedAssembliesWhichWereNotLoaded(pluginAssembly, pluginLoadedAssemblies, loadContext.LoadFromAssemblyName))
                yield return pluginReference;

            foreach (var pluginOfPlugin in LoadThePluginsOfThePlugin(pluginDescriptor, loadContext, pluginLoadedAssemblies))
                yield return pluginOfPlugin;
        }
    }

    private IEnumerable<Assembly> LoadReferencedAssembliesWhichWereNotLoaded(Assembly assembly, HashSet<string> loaded, Func<AssemblyName, Assembly> loader)
    {
        foreach (var reference in assembly.GetReferencedAssemblies())
        {
            if (reference.Name != null && !loaded.Contains(reference.Name))
            {
                loaded.Add(reference.Name);

                if (MatchesNameFilter(reference.Name))
                {
                    var referencedAssembly = loader(reference);
                    yield return referencedAssembly;
                    foreach (var referencedByReferencedAssembly in LoadReferencedAssembliesWhichWereNotLoaded(referencedAssembly, loaded, loader))
                        yield return referencedByReferencedAssembly;
                }
            }
        }
    }

    private IEnumerable<Assembly> LoadThePluginsOfThePlugin(PluginDescriptor plugin, PluginLoadContext loadContext, HashSet<string> pluginLoadedAssemblies)
    {
        foreach (var dependency in plugin.Dependencies.Where(MatchesNameFilter))
        {
            var pluginAssembly = loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(dependency.FullPath));
            var pluginAssemblyName = pluginAssembly.GetName().Name;
            if (pluginAssemblyName == null) 
                continue;
            
            pluginLoadedAssemblies.Add(pluginAssemblyName);
            yield return pluginAssembly;
            
            foreach (var referencedAssembly in LoadReferencedAssembliesWhichWereNotLoaded(pluginAssembly, pluginLoadedAssemblies, loadContext.LoadFromAssemblyName))
                yield return referencedAssembly;
        }
    }

    private bool MatchesNameFilter(string assemblyName) => options.NameFilter.Invoke(assemblyName);

    private bool MatchesNameFilter(Plugin plugin) => MatchesPluginName(plugin.Name);
    
    private bool MatchesNameFilter(PluginDescriptor plugin) => MatchesPluginName(plugin.Name);

    private bool MatchesPluginName(string pluginName)
    {
        int lastSlashIndex = pluginName.LastIndexOf('\\');
        if (lastSlashIndex != -1)
            pluginName = pluginName.Substring(lastSlashIndex + 1);
        return options.NameFilter.Invoke(pluginName);
    }
}