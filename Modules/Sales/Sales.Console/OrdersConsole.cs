using Contracts.ConsoleUi;
using System;
using AppBoot.DependencyInjection;
using ConsoleUi;
using Contracts.Sales.OrderingService;

namespace Sales.Console;

[Service(typeof(IConsoleCommand))]
public class OrdersConsole(IConsole console, IOrderingService orderingService) : IConsoleCommand
{
    public void Execute()
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

    public string MenuLabel => "Show All Orders";
}