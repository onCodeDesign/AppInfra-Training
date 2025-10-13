using AppBoot.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace AppBoot;

/// <summary>
///     A class that starts the application and initializes it.
/// </summary>
public sealed class Bootstrapper(IEnumerable<Assembly> applicationAssemblies, IDependencyContainer container) : IBootstrapper, IDisposable
{
    private readonly List<IRegistrationBehavior> behaviors = new();

    public IEnumerable<Assembly> ApplicationAssemblies { get; } = applicationAssemblies;

    public IBootstrapper AddRegistrationBehavior(IRegistrationBehavior behavior)
    {
        behaviors.Add(behavior);
        return this;
    }

    public IBootstrapper Run()
    {
        IEnumerable<Type> appTypes = ApplicationAssemblies.SelectMany(a => a.GetTypes());

        RegistrationsCatalog catalog = new RegistrationsCatalog();
        foreach (Type type in appTypes)
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                IRegistrationBehavior behavior = behaviors[i];

                IEnumerable<ServiceDescriptor> registrations = behavior.GetServicesFrom(type);
                foreach (ServiceDescriptor reg in registrations)
                {
                    catalog.Add(reg, i);
                }
            }
        }

        foreach (ServiceDescriptor registration in catalog)
        {
            container.RegisterService(registration);
        }

        container.RegisterService(new ServiceDescriptor(typeof(Application), typeof(Application), ServiceLifetime.Singleton));

        return this;
    }

    public IHost Init(IHost host)
    {
        Application? app = host.Services.GetService<Application>();
        app?.Initialize(host);
        return host;
    }

    public void Dispose()
    {
        IDisposable? disposable = container as IDisposable;
        disposable?.Dispose();
    }
}