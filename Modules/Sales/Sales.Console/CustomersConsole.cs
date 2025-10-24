using AppBoot.DependencyInjection;
using ConsoleUi;
using Contracts.ConsoleUi;
using Contracts.Sales.CustomerOrdersService;

namespace Sales.Console;

[Service(typeof(IConsoleCommand))]
class CustomersConsole(IConsole console, ICustomerOrdersService customerOrdersService) : IConsoleCommand
{
    public void Execute()
    {
        CustomerData[] customers = customerOrdersService.GetCustomersWithOrders().ToArray();

        console.WriteLine($"Customers with orders: ");
        foreach (CustomerData customer in customers)
        {
            console.WriteEntity(customer);
        }
    }

    public string MenuLabel => "Show customers with orders";
}