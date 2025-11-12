using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;
using Contracts.Sales;

namespace Sales.ConsoleCommands;

[Service(typeof(IConsoleCommand))]
internal sealed class CustomersWithOrdersContainingConsoleCommand : IConsoleCommand
{
    private readonly IConsole console;
    private readonly ICustomerService customerService;

    public CustomersWithOrdersContainingConsoleCommand(IConsole console, ICustomerService customerService)
    {
        this.console = console;
        this.customerService = customerService;
    }

    public string MenuLabel => "Show customers with orders (name contains)";

    public void Execute()
    {
        string fragment = console.AskInput("Enter substring to search in company name: ");
        if (fragment == null) fragment = string.Empty;

        var customers = customerService.GetCustomersWithOrdersContaining(fragment);

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
