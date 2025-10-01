using System.Diagnostics;

namespace AppBoot.AssemblyLoad;

/*****
 * THIS CODE IS FOR DEMO / TRAINING PURPOSES ONLY.
 * It is not throughly tested nor optimal for production.
 */

internal class BreadcrumbNameConventionPathBuilder(string hostAssemblyLocation, string pluginsDir, string[] topDirs) : IPluginPathBuilder
{
    private readonly string binFolder = "bin";
    private readonly HashSet<string> topDirsSet = new(topDirs, StringComparer.InvariantCultureIgnoreCase);

    public string GetPluginFullPath(string plugin)
    {
        plugin = plugin.Replace('\\', Path.DirectorySeparatorChar);

        var (binDebugPath, rootPath) = GoUpSrcRoot(plugin);
        string pluginPath = string.Join(Path.DirectorySeparatorChar, GetBreadcrumbPathParts(plugin));

        return Path.Combine(rootPath,
                    Path.Combine(pluginsDir,
                        Path.Combine(pluginPath,
                            Path.Combine(plugin, 
                                Path.Combine(binDebugPath, GetFileName(plugin))
                            ))));
    }

    private (string binDebugPath, string currentPath) GoUpSrcRoot(string plugin)
    {
        Stack<string> parts = new Stack<string>();
        string currentDir;
        string currentPath = Path.GetDirectoryName(hostAssemblyLocation) ?? Throw(plugin);
        bool binFound = false;
        do
        {
            string upperPath = Path.GetDirectoryName(currentPath) ?? Throw(plugin);

            currentDir = GetCurrentDir(currentPath, upperPath);
            if (!binFound)
            {
                parts.Push(currentDir);
                binFound = string.Equals(currentDir, binFolder, StringComparison.InvariantCultureIgnoreCase);
            }

            currentPath = upperPath;
        } while (!topDirsSet.Contains(currentDir));

        string binDebugPath = string.Join(Path.DirectorySeparatorChar, parts);
        return (binDebugPath, currentPath );
    }

    private static string GetCurrentDir(string currentPath, string upperPath)
    {
        string currentDir;
        if (upperPath.Length == 0)
            currentDir = currentPath;
        else if (upperPath.Length == 1 && upperPath[0] == Path.DirectorySeparatorChar)
            currentDir = currentPath.Substring(upperPath.Length);
        else
            currentDir = currentPath.Substring(upperPath.Length + 1);
        return currentDir;
    }

    private string[] GetBreadcrumbPathParts(string plugin)
    {
        int lastSlashIndex = plugin.LastIndexOf(Path.DirectorySeparatorChar);
        if (lastSlashIndex != -1)
            plugin = plugin.Substring(lastSlashIndex + 1);

        string[] parts = plugin.Split('.');
        return parts[..1];
    }

    private string GetFileName(string plugin)
    {
        int lastSeparatorIndex = plugin.LastIndexOf(Path.DirectorySeparatorChar);
        if (lastSeparatorIndex != -1)
            plugin = plugin.Substring(lastSeparatorIndex + 1);
        return $"{plugin}.dll";
    }

    private string Throw(string plugin)
    {
        throw new InvalidOperationException($"Host folder cannot be determined based on the naming convention: {plugin}");
    }
}