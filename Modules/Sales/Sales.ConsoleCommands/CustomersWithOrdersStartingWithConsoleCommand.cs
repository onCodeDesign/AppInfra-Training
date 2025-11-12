using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;
using Contracts.Sales;

namespace Sales.ConsoleCommands;

[Service(typeof(IConsoleCommand))]
internal sealed class CustomersWithOrdersStartingWithConsoleCommand : IConsoleCommand
{
    private readonly IConsole console;
    private readonly ICustomerService customerService;

    public CustomersWithOrdersStartingWithConsoleCommand(IConsole console, ICustomerService customerService)
    {
        this.console = console;
        this.customerService = customerService;
    }

    public string MenuLabel => "Show customers with orders (name starts with)";

    public void Execute()
    {
        string prefix = console.AskInput("Enter starting string for company name: ");
        if (prefix == null) prefix = string.Empty;

        var customers = customerService.GetCustomersWithOrdersStartingWith(prefix);

        if (customers.Length == 0)
        {
            console.WriteLine("No customers found.");
            return;
        }

        foreach (var c in customers)
        {
            console.WriteEntity(c);
        }
    }
}
