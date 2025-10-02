namespace Contracts.Notifications;

public interface IAmAliveSubscriber<in T>
{
	void AmAlive(T module);
}