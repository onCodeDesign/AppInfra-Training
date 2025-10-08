using AppBoot.DependencyInjection;
using ConsoleUi;
using Contracts.Notifications;
using Sales.DataModel.SalesLT;

namespace Sales.Console;

[Service(typeof(IStateChangeSubscriber<SalesOrderHeader>))]
public class SalesOrderHeaderChangeSubscriber(IConsole console) : IStateChangeSubscriber<SalesOrderHeader>
{
    public void NewItem(SalesOrderHeader item)
    {
        console.WriteLine($"  -- New Order: {item.AccountNumber}");
    }

    public void NotifyDeleted(SalesOrderHeader item)
    {
        console.WriteLine($"  -- Deleting Order: {item.AccountNumber}");
    }

    public void NotifyChanged(SalesOrderHeader item)
    {
        console.WriteLine($"  -- Changing Order: {item.AccountNumber}");
    }
}