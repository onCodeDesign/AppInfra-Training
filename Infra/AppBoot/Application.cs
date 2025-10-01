using AppBoot.SystemEx.Priority;
using Microsoft.Extensions.Hosting;

namespace AppBoot;

internal sealed class Application(IEnumerable<IModule> modules)
{
    public IEnumerable<IModule> Modules { get; } = modules.OrderByPriority();

    public void Initialize(IHost host)
    {
        foreach (IModule module in Modules)
            module.Initialize(host);
    }
}