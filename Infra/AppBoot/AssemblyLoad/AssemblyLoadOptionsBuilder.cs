using System;

namespace AppBoot.AssemblyLoad;

public class AssemblyLoadOptionsBuilder
{
    private readonly List<Plugin> addedPlugins = new();

    public Func<string, bool> NameFilter { get; set; } = s => true;

    public IPluginPathBuilder? PluginPathBuilder { get; set; } = null;
    public PluginPathBuilderOption PluginPathBuilderOption { get; set; } = PluginPathBuilderOption.SameRootPlugins;
    public string BreadcrumbNameConventionPathBuilderPluginsDir { get; set; } = string.Empty;
    public string[] BreadcrumbNameConventionPathBuilderTopDirs { get; set; } = [];

    public AssemblyLoadOptionsBuilder AddPlugin(string name)
    {
        addedPlugins.Add(new Plugin {Name = name});
        return this;
    }

    public AssemblyLoadOptionsBuilder AddPlugin(string name, params string[] dependencies)
    {
        addedPlugins.Add(new Plugin {Name = name, Dependencies = dependencies});
        return this;
    }

    public static AssemblyLoadOptions Build(Action<AssemblyLoadOptionsBuilder>? loadOptions)
    {
        var builder = new AssemblyLoadOptionsBuilder();
        loadOptions?.Invoke(builder);

        var toReturn = new AssemblyLoadOptions
        {
            Plugins = builder.addedPlugins.ToArray(),
            NameFilter = builder.NameFilter,
            PluginPathBuilder = builder.PluginPathBuilder,
            PluginPathBuilderOption = builder.PluginPathBuilderOption,
            BreadcrumbNameConventionPathBuilderPluginsDir = builder.BreadcrumbNameConventionPathBuilderPluginsDir,
            BreadcrumbNameConventionPathBuilderTopDirs = builder.BreadcrumbNameConventionPathBuilderTopDirs
        };

        return toReturn;
    }

    
}

