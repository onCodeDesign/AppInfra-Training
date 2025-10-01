using System;

namespace AppBoot.AssemblyLoad;

public class AssemblyLoadOptions
{
    public required Func<string, bool> NameFilter { get; set; }
    public IPluginPathBuilder? PluginPathBuilder { get; set; } 
    public required Plugin[] Plugins { get; set; }
    public PluginPathBuilderOption PluginPathBuilderOption { get; set; }
    public required string[] BreadcrumbNameConventionPathBuilderTopDirs { get; set; }
    public required string BreadcrumbNameConventionPathBuilderPluginsDir { get; set; }
}
 
public class Plugin
{
    public string Name { get; set; } = string.Empty;
    public string[] Dependencies { get; set; } = [];
}