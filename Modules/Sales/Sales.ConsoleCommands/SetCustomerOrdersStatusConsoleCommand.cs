using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;
using Contracts.Sales;

namespace Sales.ConsoleCommands;

[Service(typeof(IConsoleCommand))]
internal sealed class SetCustomerOrdersStatusConsoleCommand : IConsoleCommand
{
    private readonly IConsole console;
    private readonly IOrderingService orderingService;

    public SetCustomerOrdersStatusConsoleCommand(IConsole console, IOrderingService orderingService)
    {
        this.console = console;
        this.orderingService = orderingService;
    }

    public string MenuLabel => "Set status for all orders of a customer";

    public void Execute()
    {
        string customerLastName = console.AskInput("Enter customer last name: ");
        if (string.IsNullOrWhiteSpace(customerLastName))
        {
            console.WriteLine("Customer name is required.");
            return;
        }

        console.WriteLine("Choose new status (number):");
        console.WriteLine($"1 - InProcess");
        console.WriteLine($"2 - Approved");
        console.WriteLine($"3 - Backordered");
        console.WriteLine($"4 - Rejected");
        console.WriteLine($"5 - Shipped");
        console.WriteLine($"6 - Cancelled");

        string input = console.AskInput("Enter status number: ");
        if (!byte.TryParse(input, out byte status))
        {
            console.WriteLine("Invalid status.");
            return;
        }

        int changed = orderingService.SetOrdersStatus(customerLastName, status);

        console.WriteLine($"Updated {changed} orders for customer {customerLastName}.");
    }
}
