using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;
using Contracts.Notifications;
using Sales.DataModel.SalesLT;

namespace Sales.ConsoleCommands.StateChangeSubscribers;

[Service(typeof(IStateChangeSubscriber<SalesOrderHeader>))]
internal sealed class SalesOrderStateChangeConsoleSubscriber(IConsole console) : IStateChangeSubscriber<SalesOrderHeader>
{
    public void NewItem(SalesOrderHeader item)
    {
        console.WriteLine($"[SalesOrder Created] ID: {item.SalesOrderID}, Number: {item.SalesOrderNumber}, Customer: {item.CustomerID}");
    }

    public void NotifyDeleted(SalesOrderHeader item)
    {
        console.WriteLine($"[SalesOrder Deleted] ID: {item.SalesOrderID}, Number: {item.SalesOrderNumber}");
    }

    public void NotifyChanged(SalesOrderHeader item)
    {
        console.WriteLine($"[SalesOrder Changed] ID: {item.SalesOrderID}, Number: {item.SalesOrderNumber}, Status: {item.Status}");
    }
}
