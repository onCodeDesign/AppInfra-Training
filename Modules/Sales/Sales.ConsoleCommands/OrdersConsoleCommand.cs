using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;
using Contracts.Sales;

namespace Sales.ConsoleCommands;

[Service(typeof(IConsoleCommand))]
internal sealed class OrdersConsoleCommand : IConsoleCommand
{
    private readonly IConsole console;
    private readonly IOrderingService orderingService;

    public OrdersConsoleCommand(IConsole console, IOrderingService orderingService)
    {
        this.console = console;
        this.orderingService = orderingService;
    }

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
