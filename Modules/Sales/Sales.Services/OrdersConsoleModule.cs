using AppBoot;
using AppBoot.DependencyInjection;
using ConsoleUi;
using Contracts.Sales;
using Microsoft.Extensions.Hosting;

namespace Sales.Services;

[Service(typeof(IModule))]
class OrdersConsoleModule(IConsole console, IOrderingService orderingService) : IModule
{
    public void Initialize(IHost host)
    {
        console.WriteLine("OrdersConsole: Show all orders function");
        string customerName = console.AskInput("Enter customer last name: ");

        SalesOrderInfo[] orders = orderingService.GetOrdersInfo(customerName);

        console.WriteLine($"Orders for customer {customerName}: "); //Test data: Abel | Smith | Adams
        foreach (SalesOrderInfo salesOrderInfo in orders)
        {
            console.WriteEntity(salesOrderInfo);
        }
    }
}