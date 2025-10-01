using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AppBoot.AssemblyLoad;

internal interface IPluginPathBuilderFactory
{
    IPluginPathBuilder CreatePluginPathBuilder();
}

internal class PluginPathBuilderFactory(IServiceProvider serviceProvider, AssemblyLoadOptions options) : IPluginPathBuilderFactory
{
    private readonly IHostEnvironment? environment = serviceProvider.GetService<IHostEnvironment>();
    private readonly ILogger logger = serviceProvider.GetService<ILogger<PluginPathBuilderFactory>>() ?? NullLogger<PluginPathBuilderFactory>.Instance;

    public IPluginPathBuilder CreatePluginPathBuilder()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        Debug.Assert(entryAssembly != null, nameof(entryAssembly) + " != null");

        if (options.PluginPathBuilder != null)
            return options.PluginPathBuilder;

        if (options.PluginPathBuilderOption == PluginPathBuilderOption.None)
        {
            if (IsDevelopmentEnv)
                logger.LogWarning("AppBoot: PluginPathBuilderOption is set to None in Development environment. The plugins may not load, as the default path builder is used! ");
        }

        if (options.PluginPathBuilderOption == PluginPathBuilderOption.BreadcrumbNameConvention)
            return new BreadcrumbNameConventionPathBuilder(entryAssembly.Location, options.BreadcrumbNameConventionPathBuilderPluginsDir, options.BreadcrumbNameConventionPathBuilderTopDirs);

        return new SameRootPluginsNameConventionPathBuilder(entryAssembly.Location);
    }

    private bool IsDevelopmentEnv => environment != null && environment.IsDevelopment();
}

public enum PluginPathBuilderOption
{
    None,
    SameRootPlugins,
    BreadcrumbNameConvention
}
