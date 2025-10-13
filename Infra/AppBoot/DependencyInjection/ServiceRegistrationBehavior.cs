using Microsoft.Extensions.DependencyInjection;

namespace AppBoot.DependencyInjection;

/// <summary>
///     Gives service descriptor, based on the ServiceAttribute
///     If more service attribute decorations are on current type more ServiceInfo are returned, one for each attribute
/// </summary>
public sealed class ServiceRegistrationBehavior : IRegistrationBehavior
{
    public IEnumerable<ServiceDescriptor> GetServicesFrom(Type type)
    {
        var typeName = type.Name;

        var attributes = type.GetCustomAttributes(typeof(ServiceAttribute), false).Cast<ServiceAttribute>();

        return attributes.Select(a => new ServiceDescriptor(a.ExportType, type, a.Lifetime));
    }
}