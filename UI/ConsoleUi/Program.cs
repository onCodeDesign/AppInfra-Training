using AppBoot;
using AppBoot.AssemblyLoad;
using AppBoot.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Booting up..."); Console.WriteLine();

IBootstrapper bootstrapper = null!;

Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
	bootstrapper = services.AddAppBoot(options =>
	{
		options.NameFilter = assembly => assembly.StartsWith("Common")
		                                 || assembly.StartsWith("ConsoleUi")
		                                 || assembly.StartsWith("DataAccess")
		                                 || assembly.StartsWith("Export.")
		                                 || assembly.StartsWith("Notifications.")
		                                 || assembly.StartsWith("Sales.");

		options.PluginPathBuilderOption = PluginPathBuilderOption.BreadcrumbNameConvention;
		options.BreadcrumbNameConventionPathBuilderPluginsDir = "Modules";
		options.BreadcrumbNameConventionPathBuilderTopDirs = ["UI", "Modules", "Infra"];

		options.AddPlugin("Notifications.Services");

	})
	.AddRegistrationBehavior(new ServiceRegistrationBehavior())
	.Run();
})
.Build()
.InitHostedApp(bootstrapper);


Console.WriteLine(); Console.WriteLine("AppBoot done!");