using AppBoot.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace AppBoot;

public interface IBootstrapper
{
    IEnumerable<Assembly> ApplicationAssemblies { get; }
    IBootstrapper Run();
    IBootstrapper AddRegistrationBehavior(IRegistrationBehavior behavior);
    IHost Init(IHost host);
}
