using System.Diagnostics;
using AppBoot.AssemblyLoad;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppBoot;

public static class AppBootConfigureExtensions
{
	public static IBootstrapper AddAppBoot(this IServiceCollection services, Action<AssemblyLoadOptionsBuilder>? buildAction = null)
	{
		IServiceProvider serviceProvider = services.BuildServiceProvider();

		AssemblyLoadOptions options = AssemblyLoadOptionsBuilder.Build(buildAction);

		IPluginPathBuilderFactory builderFactory = new PluginPathBuilderFactory(serviceProvider, options);
		var loader = new AssembliesLoader(options, builderFactory.CreatePluginPathBuilder());
		var assemblies = loader.LoadAssemblies().ToList();

		var bootstrapper = new Bootstrapper(assemblies, new MsDependencyInjectionAdapter(services));
		return bootstrapper;
	}


	public static IHost InitHostedApp(this IHost app, IBootstrapper bootstrapper)
	{
		Debug.Assert(bootstrapper != null);
		return bootstrapper.Init(app);
	}
}