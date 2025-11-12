using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;
using Contracts.Sales;

namespace Sales.ConsoleCommands;

[Service(typeof(IConsoleCommand))]
internal sealed class CustomersWithOrdersConsoleCommand(IConsole console, ICustomerService customerService) : IConsoleCommand
{
    public string MenuLabel => "Show customers with orders";

    public void Execute()
    {
        console.WriteLine("CustomersWithOrders: retrieving customers with orders...");

        CustomerData[] customers = customerService.GetCustomersWithOrders();

        if (customers.Length == 0)
        {
            console.WriteLine("No customers with orders found.");
            return;
        }

        foreach (var c in customers)
        {
            console.WriteEntity(c);
        }
    }
}
