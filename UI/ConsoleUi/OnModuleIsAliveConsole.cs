using AppBoot;
using AppBoot.DependencyInjection;
using Contracts.Notifications;

namespace ConsoleUi;

[Service(typeof(IAmAliveSubscriber<>))]
public class OnModuleIsAliveConsole<TModule> : IAmAliveSubscriber<IModule>
	where TModule: IModule
{
	public void AmAlive(IModule module)
	{
		Console.WriteLine($"{module.Name} is alive!");
	}
}

