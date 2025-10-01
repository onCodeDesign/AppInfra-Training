using System.Reflection;
using System.Runtime.Loader;

namespace AppBoot.AssemblyLoad
{
    class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver[] resolvers;

        public PluginLoadContext(string name, string[] pluginPaths)
            : base(name)
        {
            resolvers = new AssemblyDependencyResolver[pluginPaths.Length];
            for (int i = 0; i < pluginPaths.Length; i++)
            {
                resolvers[i] = new AssemblyDependencyResolver(pluginPaths[i]);
            }
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            for (int i = 0; i < resolvers.Length; i++)
            {
                string? assemblyPath = resolvers[i].ResolveAssemblyToPath(assemblyName);
                if (assemblyPath != null)
                {
                    return LoadFromAssemblyPath(assemblyPath);
                }
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            for (int i = 0; i < resolvers.Length; i++)
            {
                string? libraryPath = resolvers[i].ResolveUnmanagedDllToPath(unmanagedDllName);
                if (libraryPath != null)
                {
                    return LoadUnmanagedDllFromPath(libraryPath);
                }
            }

            return IntPtr.Zero;
        }
    }
}
