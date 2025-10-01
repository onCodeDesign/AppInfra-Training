using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AppBoot.DependencyInjection;

/// <summary>
///     Gives service descriptor, based on the ServiceAttribute
///     If more service attribute decorations are on current type more ServiceInfo are returned, one for each attribute
/// </summary>
public sealed class ServiceRegistrationBehavior : IRegistrationBehavior
{
	public IEnumerable<ServiceDescriptor> GetServicesFrom(Type type)
	{
		string typeName = type.Name;

		IEnumerable<ServiceAttribute> attributes = type.GetCustomAttributes(typeof(ServiceAttribute), false).Cast<ServiceAttribute>();
		return attributes.Select(a => new ServiceDescriptor(a.ExportType ?? type, type, a.Lifetime));
	}
}

public class ConventionRegistrationBehavior : IRegistrationBehavior
{
	private readonly IList<ServiceBuilder> builders = new List<ServiceBuilder>();

	public IEnumerable<ServiceDescriptor> GetServicesFrom(Type type)
	{
		IEnumerable<ServiceDescriptor> services = builders.SelectMany(x => x.GetServicesFrom(type));
		return services;
	}

	public ServiceBuilder ForType(Type type)
	{
		ServiceBuilder builder = CreateServiceBuilder(x => x == type);
		builders.Add(builder);
		return builder;
	}

	[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Convenience call")]
	public ServiceBuilder ForType<T>()
	{
		return this.ForType(typeof(T));
	}

	public ServiceBuilder ForTypesDerivedFrom(Type type)
	{
		ServiceBuilder builder = CreateServiceBuilder(x => type.IsAssignableFrom(x));
		builders.Add(builder);
		return builder;
	}

	[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Convenience call")]
	public ServiceBuilder ForTypesDerivedFrom<T>()
	{
		return ForTypesDerivedFrom(typeof(T));
	}

	public ServiceBuilder ForTypesMatching(Predicate<Type> typeFilter)
	{
		ServiceBuilder builder = CreateServiceBuilder(typeFilter);
		builders.Add(builder);
		return builder;
	}


	private static ServiceBuilder CreateServiceBuilder(Predicate<Type> typeFilter)
	{
		return new ServiceBuilder(typeFilter);
	}
}