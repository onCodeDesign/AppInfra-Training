using AppBoot.DependencyInjection;
using Contracts.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Notifications.Services;

[Service(typeof (INotificationService), ServiceLifetime.Singleton)]
public class NotificationService(IServiceProvider serviceProvider) : INotificationService
{
    public void NotifyAlive<T>(T item)
    {
        var subscribers = serviceProvider.GetServices<IAmAliveSubscriber<T>>();
        foreach (var subscriber in subscribers)
        {
            subscriber.AmAlive(item);
        }
    }

    public void NotifyNew<T>(T item)
    {
        NotifyStateChangeSubscribers<T>(subscriber => subscriber.NewItem(item));
    }

    public void NotifyDeleted<T>(T item)
    {
        NotifyStateChangeSubscribers<T>(subscriber => subscriber.NotifyDeleted(item));
    }

    public void NotifyChanged<T>(T item)
    {
        NotifyStateChangeSubscribers<T>(subscriber => subscriber.NotifyChanged(item));
    }

    private void NotifyStateChangeSubscribers<T>(Action<IStateChangeSubscriber<T>> notification)
    {
        var subscribers = serviceProvider.GetServices<IStateChangeSubscriber<T>>();
        foreach (var subscriber in subscribers)
        {
            notification(subscriber);
        }
    }

    public void NotifyStatusChange<T>(T item, Status newStatus, Status oldStatus)
    {
        throw new System.NotImplementedException();
    }
}