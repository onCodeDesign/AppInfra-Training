using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;
using Contracts.PersonsManagement;

namespace PersonsManagement.ConsoleCommands;

[Service(typeof(IConsoleCommand))]
internal sealed class AddPersonConsoleCommand(
    IConsole console,
    IEntityReader entityReader,
    IPersonService personService) : IConsoleCommand
{
    public string MenuLabel => "Add new Person";

    public void Execute()
    {
        console.WriteLine("=== Add New Person ===");
        console.WriteLine("");

        var reader = entityReader.BeginEntityRead<PersonDto>();

        console.WriteLine("Please enter values for the following fields:");
        console.WriteLine("(Press Enter to skip optional fields)");
        console.WriteLine("");

        foreach (var fieldName in reader.GetFields())
        {
            string value = console.AskInput($"{fieldName}: ");
            reader.SetFieldValue(fieldName, value);
        }

        var personDto = reader.GetEntity();

        console.WriteLine("");
        console.WriteLine("=== Person Data ===");
        console.WriteLine($"Name: {personDto.Title} {personDto.FirstName} {personDto.MiddleName} {personDto.LastName} {personDto.Suffix}".Trim());
        console.WriteLine($"Email: {personDto.EmailAddress ?? "(none)"}");
        console.WriteLine($"Phone: {personDto.Phone ?? "(none)"}");
        console.WriteLine($"Company: {personDto.CompanyName ?? "(none)"}");
        console.WriteLine("");

        // Map DTO to contract data
        var personData = new PersonData
        {
            NameStyle = false,
            Title = personDto.Title,
            FirstName = personDto.FirstName,
            MiddleName = personDto.MiddleName,
            LastName = personDto.LastName,
            Suffix = personDto.Suffix,
            EmailAddress = personDto.EmailAddress,
            Phone = personDto.Phone,
            CompanyName = personDto.CompanyName
        };

        try
        {
            int newPersonId = personService.AddPerson(personData);
            console.WriteLine($"✓ Person saved successfully! ID: {newPersonId}");
            console.WriteLine("");
        }
        catch (Exception ex)
        {
            console.WriteLine($"✗ Error saving Person: {ex.Message}");
        }
    }
}

// DTO for reading Person data from console
public class PersonDto
{
    public string? Title { get; set; }
    public string FirstName { get; set; } = "";
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = "";
    public string? Suffix { get; set; }
    public string? EmailAddress { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
}
