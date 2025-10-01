namespace AppBoot.AssemblyLoad;

public interface IPluginPathBuilder
{
	string GetPluginFullPath(string plugin);
}