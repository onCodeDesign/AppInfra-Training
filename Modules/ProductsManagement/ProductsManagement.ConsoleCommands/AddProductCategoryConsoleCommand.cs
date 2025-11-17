using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;

namespace ProductsManagement.ConsoleCommands;

[Service(typeof(IConsoleCommand))]
internal sealed class AddProductCategoryConsoleCommand : IConsoleCommand
{
    private readonly IConsole console;
    private readonly IEntityReader entityReader;

    public AddProductCategoryConsoleCommand(
        IConsole console, 
        IEntityReader entityReader)
    {
        this.console = console;
        this.entityReader = entityReader;
    }

    public string MenuLabel => "Add new Product Category";

    public void Execute()
    {
        console.WriteLine("=== Add New Product Category ===");
        console.WriteLine("");

        var reader = entityReader.BeginEntityRead<ProductCategoryDto>();

        console.WriteLine("Please enter values for the following fields:");
        console.WriteLine("(Press Enter to skip optional fields)");
        console.WriteLine("");

        foreach (var fieldName in reader.GetFields())
        {
            string value = console.AskInput($"{fieldName}: ");
            reader.SetFieldValue(fieldName, value);
        }

        var categoryDto = reader.GetEntity();

        console.WriteLine("");
        console.WriteLine("=== Read Product Category Data ==="); 
        console.WriteLine($"Name: {categoryDto.Name}");
        console.WriteLine($"Parent Category ID: {categoryDto.ParentProductCategoryId.ToString()}");
        console.WriteLine("");
    }
}

// DTO for reading ProductCategory data from console
public class ProductCategoryDto
{
    public int ParentProductCategoryId { get; set; }
    public string Name { get; set; } = "";
}
