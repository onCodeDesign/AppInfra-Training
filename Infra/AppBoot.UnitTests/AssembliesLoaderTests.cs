using System.Reflection;
using AppBoot.AssemblyLoad;

namespace AppBoot.UnitTests;

public class AssembliesLoaderTests
{
    private static readonly Assembly thisAssembly = typeof(AssembliesLoaderTests).Assembly;

    [Fact]
    public void LoadAssemblies_NoPlugins_CurrentAssemblyLoaded()
    {
        AssembliesLoader target = GetTarget();

        IEnumerable<Assembly> actual = target.LoadAssemblies();

        Assert.Contains(actual, assembly => assembly == thisAssembly);
    }

    [Fact]
    public void LoadAssemblies_NoPlugins_CurrentAndReferencedAssembliesLoadedAndFiltered()
    {
        AssembliesLoader target = GetTarget(s => s.StartsWith("AppBoot"), () => [thisAssembly]);

        IEnumerable<Assembly> actual = target.LoadAssemblies();

        var actualNames = actual.Select(a => a.GetName().Name).ToList();
        AssertEx.AreEquivalent(actualNames, "AppBoot", "AppBoot.UnitTests"
            );
    }

    private AssembliesLoader GetTarget()
    {
        return GetTarget(s => true);
    }



    private AssembliesLoader GetTarget(Func<string, bool> nameFilter, Func<Assembly[]> domainAssembliesProvider)
    {
        return new AssembliesLoader(new AssemblyLoadOptions
            {
                NameFilter = nameFilter,
                Plugins = [],
                BreadcrumbNameConventionPathBuilderTopDirs = [],
                BreadcrumbNameConventionPathBuilderPluginsDir = string.Empty
            },
            new DummyBuilder(string.Empty),
            domainAssembliesProvider
        );
    }
    private AssembliesLoader GetTarget(Func<string, bool> nameFilter)
    {
        return new AssembliesLoader(new AssemblyLoadOptions
            {
                NameFilter = nameFilter,
                Plugins = [],
                BreadcrumbNameConventionPathBuilderTopDirs = [],
                BreadcrumbNameConventionPathBuilderPluginsDir = string.Empty
            },
            new DummyBuilder(string.Empty)
        );
    }

    internal class DummyBuilder : IPluginPathBuilder
    {
        private readonly string path;

        public DummyBuilder(string path)
        {
            this.path = path;
        }

        public string GetPluginFullPath(string plugin)
        {
            return path;
        }
    }
}