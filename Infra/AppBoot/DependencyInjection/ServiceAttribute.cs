using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace AppBoot.DependencyInjection;

/// <summary>
///     Declares a service implementation, by decorating the class that implements it.
///     It may also specify the lifetime of the service instance by using the Lifetime enum
/// </summary>
[SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute may be inherited by client applications to extend the registration behaviors")]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ServiceAttribute : Attribute
{
    public ServiceAttribute(Type exportType, ServiceLifetime lifetime)
    {
        ExportType = exportType;
        Lifetime = lifetime;
    }

    public ServiceAttribute(Type exportType)
		: this(exportType, ServiceLifetime.Transient)
	{
	}

	public Type ExportType { get; private set; }

	public ServiceLifetime Lifetime { get; private set; }
}