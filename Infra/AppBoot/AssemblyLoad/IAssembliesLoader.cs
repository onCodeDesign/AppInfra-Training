using System.Reflection;

namespace AppBoot.AssemblyLoad;

internal interface IAssembliesLoader
{
	IEnumerable<Assembly> LoadAssemblies();
}