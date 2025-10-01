using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace AppBoot.DependencyInjection;

public class ExportBuilder
{
	private Type? contractType;
	private ServiceLifetime life = ServiceLifetime.Transient;

	internal ExportBuilder(Type fromType)
	{
		FromType = fromType;
	}

	public Type FromType { get; private set; }

	public ExportBuilder AsContractType(Type type)
	{
		this.contractType = type;
		return this;
	}

	[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Convenience call")]
	public ExportBuilder AsContractType<T>()
	{
		return AsContractType(typeof(T));
	}

	public ExportBuilder WithServiceLifetime(ServiceLifetime lifetime)
	{
		this.life = lifetime;
		return this;
	}

	internal ServiceDescriptor GetServiceDescriptor(Type type)
	{
		return new ServiceDescriptor(contractType ?? type, type, life);
	}
}