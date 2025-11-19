using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;
using Contracts.Sales;

namespace Sales.ConsoleCommands;

[Service(typeof(IConsoleCommand))]
internal sealed class ImportPersonsAsCustomersConsoleCommand : IConsoleCommand
{
    private readonly IConsole console;
    private readonly ICustomerImportService customerImportService;

    public ImportPersonsAsCustomersConsoleCommand(
        IConsole console,
        ICustomerImportService customerImportService)
    {
        this.console = console;
        this.customerImportService = customerImportService;
    }

    public string MenuLabel => "Import Persons as Customers";

    public void Execute()
    {
        console.WriteLine("=== Import Persons as Customers ===");
        console.WriteLine("");
        console.WriteLine("This will import all persons from the PersonsManagement module");
        console.WriteLine("as customers in the Sales module.");
        console.WriteLine("");
        console.WriteLine("Import Rules:");
        console.WriteLine("  - Match by: FirstName + LastName");
        console.WriteLine("  - ADD: If no matching customer exists");
        console.WriteLine("  - UPDATE: If person.ModifiedDate > customer.ModifiedDate");
        console.WriteLine("  - SKIP: If customer is already up-to-date");
        console.WriteLine("");

        var confirm = console.AskInput("Continue? (yes/no): ");
        if (!confirm.StartsWith("y", StringComparison.OrdinalIgnoreCase))
        {
            console.WriteLine("Import cancelled.");
            return;
        }

        console.WriteLine("");
        console.WriteLine("Starting import...");
        console.WriteLine("");

        try
        {
            var result = customerImportService.ImportPersonsAsCustomers();

            console.WriteLine("=== Import Complete! ===");
            console.WriteLine("");
            console.WriteLine($"Total Persons Processed: {result.TotalPersonsProcessed}");
            console.WriteLine($"  ✓ Customers Added:     {result.CustomersAdded}");
            console.WriteLine($"  ↻ Customers Updated:   {result.CustomersUpdated}");
            console.WriteLine($"  ⊘ Customers Skipped:   {result.CustomersSkipped}");
            console.WriteLine("");
        }
        catch (Exception ex)
        {
            console.WriteLine($"✗ Error during import: {ex.Message}");
            console.WriteLine($"   {ex.GetType().Name}");
        }
    }
}
