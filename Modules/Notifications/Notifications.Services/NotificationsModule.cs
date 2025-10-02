using AppBoot;
using AppBoot.DependencyInjection;
using Contracts.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Notifications.Services;

[Service(typeof(IModule), ServiceLifetime.Singleton)]
class NotificationsModule(INotificationService notificationService) : IModule
{
	public void Initialize(IHost host)
	{
		notificationService.NotifyAlive(this);
	}
}