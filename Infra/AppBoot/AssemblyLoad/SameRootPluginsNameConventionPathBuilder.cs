using System;

namespace AppBoot.AssemblyLoad;

/// <summary>
///   Builds the full path of the plugin based on the convention:
///     - the folder of the plugin has the name of the plugin without the extension,
///     - the host assembly folder and the plugin folder are siblings
///     - the path under the assembly folder and the plugin folder are identical
/// </summary>
internal class SameRootPluginsNameConventionPathBuilder(string hostAssemblyLocation) : IPluginPathBuilder
{
	public string GetPluginFullPath(string plugin)
	{
		string hostName = Path.GetFileNameWithoutExtension(hostAssemblyLocation);
		
		Stack<string> parts = new Stack<string>();

		string currentDir;
		string currentPath = Path.GetDirectoryName(hostAssemblyLocation) ?? Throw(plugin);
		do
		{
			string upperPath = Path.GetDirectoryName(currentPath) ?? Throw(plugin);

			currentDir = GetCurrentDir(currentPath, upperPath);
			parts.Push(currentDir);
			currentPath = upperPath;
		} while (!string.Equals(currentDir, hostName, StringComparison.InvariantCultureIgnoreCase));
		parts.Pop();
		string root = currentPath;

		string pluginName = GetPluginName(plugin);
		string pluginFolder = Path.Combine(root, pluginName);
		string pluginBinPath = string.Join(Path.DirectorySeparatorChar, parts);
		return Path.Combine(pluginFolder, pluginBinPath, $"{pluginName}.dll");
	}

	private string Throw(string plugin)
	{
		throw new InvalidOperationException($"Plugin path cannot be build based on folder naming convention: {plugin}");
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

	private string GetPluginName(string plugin)
	{
		int lastSlash = plugin.LastIndexOf('\\');
		if (lastSlash != -1)
			plugin = plugin.Substring(lastSlash + 1);
		return plugin;
	}
}