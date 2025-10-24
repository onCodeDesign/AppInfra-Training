using AppBoot.DependencyInjection;
using ConsoleUi;
using Contracts.ConsoleUi;
using Contracts.Sales.OrderingService;

namespace Sales.Console;

[Service(typeof(IConsoleCommand))]
class CancelOrdersCommand(IConsole console, IOrderingService orderingService) : IConsoleCommand
{
    public void Execute()
    {
        string customerName = console.AskInput("Enter customer last name: ");
        
        console.WriteLine("Customers orders before cancel:");
        SalesOrderInfo[] orders = orderingService.GetOrdersInfo(customerName);
        foreach (SalesOrderInfo salesOrderInfo in orders)
        {
            console.WriteEntity(salesOrderInfo);
        }

        console.WriteLine("");
        console.WriteLine("Canceling...");
        console.WriteLine("");
        orderingService.CancelCustomerOrders(customerName);

        console.WriteLine("Customers orders after cancel:");
        SalesOrderInfo[] updatedOrders = orderingService.GetOrdersInfo(customerName);
        foreach (SalesOrderInfo salesOrderInfo in updatedOrders)
        {
            console.WriteEntity(salesOrderInfo);
        }
    }

    public string MenuLabel => "Cancel Customer Orders";
}