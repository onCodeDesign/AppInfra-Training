using System.Diagnostics;

namespace AppBoot.AssemblyLoad;

[DebuggerDisplay("Name = {Name}")]
class PluginDescriptor
{
    public required string Name { get; set; }
    public required string FullPath { get; set; }
    public PluginDescriptor[] Dependencies { get; set; } = [];

    public static PluginDescriptor Create(Plugin plugin, IPluginPathBuilder pathBuilder)
    {
        var toReturn = new PluginDescriptor
        {
            Name = plugin.Name,
            FullPath = pathBuilder.GetPluginFullPath(plugin.Name),
            Dependencies = new PluginDescriptor[plugin.Dependencies.Length]
        };
        for (int i = 0; i < plugin.Dependencies.Length; i++)
        {
            var dependencyPlugin = new PluginDescriptor
            {
                Name = plugin.Dependencies[i],
                FullPath = pathBuilder.GetPluginFullPath(plugin.Dependencies[i])
            };
            toReturn.Dependencies[i] = dependencyPlugin;
        }
        return toReturn;
    }

    public IEnumerable<string> GetAllPaths()
    {
        yield return FullPath;
        foreach (var dependency in Dependencies)
            yield return dependency.FullPath;
    }
}