using System.Reflection;

namespace AppBoot.AssemblyLoad;

internal class AssembliesLoader : IAssembliesLoader
{
    private readonly Func<Assembly[]> domainAssembliesProvider;
    private readonly AssemblyLoadOptions options;
    private readonly IPluginPathBuilder pathBuilder;
    readonly HashSet<string> domainLoadedAssemblies = new HashSet<string>();

    public AssembliesLoader(AssemblyLoadOptions options, IPluginPathBuilder pathBuilder)
        : this(options, pathBuilder, AppDomain.CurrentDomain.GetAssemblies)
    {
    }

    public AssembliesLoader(AssemblyLoadOptions options, IPluginPathBuilder pathBuilder, Func<Assembly[]> domainAssembliesProvider)
    {
        this.options = options;
        this.pathBuilder = pathBuilder;
        this.domainAssembliesProvider = domainAssembliesProvider;
    }

    public IEnumerable<Assembly> LoadAssemblies()
    {
        foreach (var referencedAssembly in LoadDomainAssemblies(domainAssembliesProvider()))
            yield return referencedAssembly;
        foreach (var assembly in LoadPluginAssemblies())
            yield return assembly;
    }

    private IEnumerable<Assembly> LoadDomainAssemblies(Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            string? assemblyName = assembly.GetName().Name;
            if (assemblyName != null && MatchesNameFilter(assemblyName) && !domainLoadedAssemblies.Contains(assemblyName))
            {
                domainLoadedAssemblies.Add(assemblyName);
                yield return assembly;
                foreach (var referencedAssembly in LoadReferencedAssembliesWhichWereNotLoaded(assembly, domainLoadedAssemblies, AppDomain.CurrentDomain.Load))
                    yield return referencedAssembly;
            }
        }
    }

    private IEnumerable<Assembly> LoadReferencedAssembliesWhichWereNotLoaded(Assembly assembly, HashSet<string> loaded, Func<AssemblyName, Assembly> loader)
    {
        foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
        {
            if (referencedAssembly.Name != null && MatchesNameFilter(referencedAssembly.Name) && !loaded.Contains(referencedAssembly.Name))
            {
                var loadedAssembly = loader(referencedAssembly);
                loaded.Add(referencedAssembly.Name);
                yield return loadedAssembly;
                foreach (var referencedByReferencedAssembly in LoadReferencedAssembliesWhichWereNotLoaded(loadedAssembly, loaded, loader))
                    yield return referencedByReferencedAssembly;
            }
        }
    }

    private IEnumerable<Assembly> LoadPluginAssemblies()
    {
        if (options.Plugins == null) yield break;

        foreach (var plugin in options.Plugins.Where(MatchesNameFilter))
        {
            var pluginDescriptor = PluginDescriptor.Create(plugin, pathBuilder);
            PluginLoadContext loadContext = new PluginLoadContext(pluginDescriptor.Name, pluginDescriptor.GetAllPaths().ToArray());

            var pluginAssembly = loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginDescriptor.FullPath));
            yield return pluginAssembly;

            HashSet<string> pluginLoadedAssemblies = new(domainLoadedAssemblies);
            var pluginAssemblyName = pluginAssembly.GetName().Name;
            if (pluginAssemblyName != null) 
                pluginLoadedAssemblies.Add(pluginAssemblyName);
            
            foreach (var pluginReference in LoadReferencedAssembliesWhichWereNotLoaded(pluginAssembly, pluginLoadedAssemblies, loadContext.LoadFromAssemblyName))
                yield return pluginReference;

            foreach (var pluginOfPlugin in LoadThePluginsOfThePlugin(pluginDescriptor, loadContext, pluginLoadedAssemblies))
                yield return pluginOfPlugin;
        }
    }

    private IEnumerable<Assembly> LoadThePluginsOfThePlugin(PluginDescriptor plugin, PluginLoadContext loadContext, HashSet<string> pluginLoadedAssemblies)
    {
        foreach (var dependency in plugin.Dependencies.Where(MatchesNameFilter))
        {
            var pluginAssembly = loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(dependency.FullPath));
            var pluginAssemblyName = pluginAssembly.GetName().Name;
            if (pluginAssemblyName != null) 
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