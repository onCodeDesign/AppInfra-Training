using System.Reflection;
using System.Runtime.Loader;

namespace AppBoot.AssemblyLoad
{
    class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver[] resolvers;
        private readonly HashSet<string> loadedAssemblies;

        public PluginLoadContext(string name, string[] pluginPaths, ICollection<string> defaultAssemblies)
            : base(name)

        {
            resolvers = new AssemblyDependencyResolver[pluginPaths.Length];
            for (int i = 0; i < pluginPaths.Length; i++)
            {
                resolvers[i] = new AssemblyDependencyResolver(pluginPaths[i]);
            }

            loadedAssemblies = new HashSet<string>(defaultAssemblies);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            if (assemblyName.Name == null)
                return null;

            if (!loadedAssemblies.Contains(assemblyName.Name))
            {
                for (int i = 0; i < resolvers.Length; i++)
                {
                    string? assemblyPath = resolvers[i].ResolveAssemblyToPath(assemblyName);
                    if (assemblyPath != null)
                    {
                        Assembly ass = LoadFromAssemblyPath(assemblyPath);
                        string? assName = ass.GetName().Name;
                        if (assName != null)
                        {
                            loadedAssemblies.Add(assName);
                            //Console.WriteLine($"Assembly: {assemblyName.Name}");
                            return ass;
                        }
                    }
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
