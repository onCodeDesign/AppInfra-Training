using AppBoot;
using AppBoot.DependencyInjection;
using AppBoot.SystemEx.Priority;
using Contracts.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sales.Services;

[Service(typeof(IModule), ServiceLifetime.Singleton)]
[Priority(Priorities.High)]
class SalesModule(INotificationService notificationService) : IModule
{
    public void Initialize(IHost host)
    {
        notificationService.NotifyAlive(this);
    }
}