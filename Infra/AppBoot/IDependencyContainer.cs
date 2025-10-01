using Microsoft.Extensions.DependencyInjection;

namespace AppBoot;

/// <summary>
///     Represents an abstraction of a Dependency Injection Container
///     Existent container frameworks (like Unity) are adapted to this abstraction
/// </summary>
public interface IDependencyContainer
{
    /// <summary>
    ///     Gets this Dependency Injection Container adapted to IServiceLocator interface.
    ///     This is the interface that is going to be used to request registered service implementations
    /// </summary>
    IServiceProvider? AsServiceProvider { get; }

    /// <summary>
    /// Gets the services while it is configured
    /// </summary>
    IEnumerable<ServiceDescriptor> AppServices { get; }

    /// <summary>
    ///     Registers a type to the container based on the service information.
    /// </summary>
    void RegisterService(ServiceDescriptor service);


    /// <summary>
    ///     Registers the instance into the container as a singleton (Lifetime.Application)
    /// </summary>
    void RegisterInstance<T>(T instance);

    /// <summary>
    ///     Registers the instance into the container as a singleton (Lifetime.Application)
    /// </summary>
    void RegisterInstance(Type from, object instance);
}