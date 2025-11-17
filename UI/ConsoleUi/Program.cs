using AppBoot;
using AppBoot.AssemblyLoad;
using AppBoot.DependencyInjection;
using ConsoleUi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Booting up..."); Console.WriteLine();

IBootstrapper bootstrapper = null!;

var host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
	bootstrapper = services.AddAppBoot(options =>
	{
		options.NameFilter = assembly => assembly.StartsWith("Common")
		                                 || assembly.StartsWith("ConsoleUi")
		                                 || assembly.StartsWith("DataAccess")
		                                 || assembly.StartsWith("Export.")
		                                 || assembly.StartsWith("Notifications.")
		                                 || assembly.StartsWith("Sales.")
		                                 || assembly.StartsWith("ProductsManagement.")
		                                 || assembly.StartsWith("PersonsManagement.");

		options.PluginPathBuilderOption = PluginPathBuilderOption.BreadcrumbNameConvention;
		options.BreadcrumbNameConventionPathBuilderPluginsDir = "Modules";
		options.BreadcrumbNameConventionPathBuilderTopDirs = ["UI", "Modules", "Infra"];
		options
			.AddPlugin("Notifications.Services")
			.AddPlugin("Sales.Services", "Sales.DbContext", "Sales.ConsoleCommands")
			.AddPlugin("Export.Services")
			.AddPlugin("ProductsManagement.Services", "ProductsManagement.DbContext", "ProductsManagement.ConsoleCommands")
			.AddPlugin("PersonsManagement.Services", "PersonsManagement.DbContext")
			;

	})
	.AddRegistrationBehavior(new ServiceRegistrationBehavior())
	.Run();
})
.Build()
.InitHostedApp(bootstrapper);


Console.WriteLine(); Console.WriteLine("AppBoot done!");