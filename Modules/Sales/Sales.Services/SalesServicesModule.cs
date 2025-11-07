using AppBoot;
using AppBoot.DependencyInjection;
using Contracts.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AppBoot.SystemEx.Priority;

namespace Sales.Services;

[Priority(Priorities.High)]
[Service(typeof(IModule), ServiceLifetime.Singleton)]
class SalesServicesModule(INotificationService notificationService) : IModule
{
    public void Initialize(IHost host)
    {
        notificationService.NotifyAlive(this);
    }
}
