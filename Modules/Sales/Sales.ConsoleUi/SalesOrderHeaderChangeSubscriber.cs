using AppBoot.DependencyInjection;
using Contracts.Notifications;
using Sales.DataModel.SalesLT;

namespace Sales.ConsoleUi
{
    [Service(typeof(IStateChangeSubscriber<SalesOrderHeader>))]
    public class SalesOrderHeaderChangeSubscriber : IStateChangeSubscriber<SalesOrderHeader>
    {
        public void NewItem(SalesOrderHeader item)
        {
            Console.WriteLine($"  -- New Order: {item.AccountNumber}");
        }

        public void NotifyDeleted(SalesOrderHeader item)
        {
            Console.WriteLine($"  -- Deleting Order: {item.AccountNumber}");
        }

        public void NotifyChanged(SalesOrderHeader item)
        {
            Console.WriteLine($"  -- Changing Order: {item.AccountNumber}");
        }
    }
}
