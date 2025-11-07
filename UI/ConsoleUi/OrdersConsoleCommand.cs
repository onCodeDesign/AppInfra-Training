using AppBoot;
using AppBoot.DependencyInjection;
using Contracts.Sales;

namespace ConsoleUi;

[Service(typeof(IConsoleCommand))]
internal sealed class OrdersConsoleCommand(IConsole console, IOrderingService orderingService) : IConsoleCommand
{
    public string MenuLabel => "Show all orders";

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
}
