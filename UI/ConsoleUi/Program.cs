using AppBoot;
using AppBoot.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Booting up...");

IBootstrapper bootstrapper = null!;

Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
	bootstrapper = services.AddAppBoot(options =>
	{
		options.NameFilter = assembly => assembly.StartsWith("Common")
		                                 || assembly.StartsWith("Contracts")
		                                 || assembly.StartsWith("DataAccess")
		                                 || assembly.StartsWith("iQuarc.DataAccess")
		                                 || assembly.StartsWith("Export.")
		                                 || assembly.StartsWith("Notifications.")
		                                 || assembly.StartsWith("Sales.")
			;

	})
	.AddRegistrationBehavior(new ServiceRegistrationBehavior())
	.Run();
})
.Build()
.InitHostedApp(bootstrapper);

Console.WriteLine("AppBoot done!");