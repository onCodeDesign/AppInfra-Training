using AppBoot.DependencyInjection;

namespace AppBoot;

public static class BootstrapperConfigureExtensions
{
	public static IBootstrapper AddRegistrationBehavior(this IBootstrapper bootstrapper, IRegistrationBehavior behavior)
	{
		bootstrapper.AddRegistrationBehavior(behavior);
		return bootstrapper;
	}
}