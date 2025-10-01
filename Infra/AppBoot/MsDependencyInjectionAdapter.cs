using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppBoot;

public class MsDependencyInjectionAdapter(IServiceCollection services) : IDependencyContainer
{
	private IServiceProvider? serviceProvider;

	public void RegisterService(ServiceDescriptor service)
	{
		services.Add(service);
	}

	public void RegisterInstance<T>(T instance)
	{
		Debug.Assert(instance != null, nameof(instance) + " != null");
		services.Add(new ServiceDescriptor(typeof(T), instance));
	}

	public void RegisterInstance(Type from, object instance)
	{
		services.Add(new ServiceDescriptor(from, instance));
	}

	public IServiceProvider? AsServiceProvider => serviceProvider;

	public IEnumerable<ServiceDescriptor> AppServices => services;

	public void SetAppServiceProvider(IServiceProvider appServices)
	{
		serviceProvider = appServices;
	}
}