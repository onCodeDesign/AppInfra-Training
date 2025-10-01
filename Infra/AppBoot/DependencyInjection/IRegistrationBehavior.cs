using Microsoft.Extensions.DependencyInjection;

namespace AppBoot.DependencyInjection;

/// <summary>
///     Represents one of the behaviors used by the Bootstraper to register types into the Dependency Injection Container
/// </summary>
public interface IRegistrationBehavior
{
    /// <summary>
    ///     Gets the services information that will be registered for given type
    /// </summary>
    IEnumerable<ServiceDescriptor> GetServicesFrom(Type type);
}