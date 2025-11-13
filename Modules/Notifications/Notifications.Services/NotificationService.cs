using AppBoot.DependencyInjection;
using Contracts.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Notifications.Services;

[Service(typeof (INotificationService), ServiceLifetime.Singleton)]
public class NotificationService(IServiceProvider serviceProvider) : INotificationService
{
    public void NotifyNew<T>(T item)
    {
        var subscribers = serviceProvider.GetServices<IStateChangeSubscriber<T>>();
        foreach (var subscriber in subscribers)
        {
            subscriber.NewItem(item);
        }
    }

    public void NotifyAlive<T>(T item)
    {
        var subscribers = serviceProvider.GetServices<IAmAliveSubscriber<T>>();
        foreach (var subscriber in subscribers)
        {
            subscriber.AmAlive(item);
        }
    }

    public void NotifyDeleted<T>(T item)
    {
        var subscribers = serviceProvider.GetServices<IStateChangeSubscriber<T>>();
        foreach (var subscriber in subscribers)
        {
            subscriber.NotifyDeleted(item);
        }
    }

    public void NotifyChanged<T>(T item)
    {
        var subscribers = serviceProvider.GetServices<IStateChangeSubscriber<T>>();
        foreach (var subscriber in subscribers)
        {
            subscriber.NotifyChanged(item);
        }
    }

    public void NotifyStatusChange<T>(T item, Status newStatus, Status oldStatus)
    {
        throw new System.NotImplementedException();
    }
}