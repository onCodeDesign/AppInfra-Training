using AppBoot;
using AppBoot.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace ConsoleUi;

[Service(typeof(IModule))]
internal sealed class ConsoleUiModule : IModule
{
    private readonly IConsole console;
    private readonly IEnumerable<IConsoleCommand> commands;

    public ConsoleUiModule(IConsole console, IEnumerable<IConsoleCommand> commands)
    {
        this.console = console;
        this.commands = commands;
    }

    public void Initialize(IHost host)
    {
        // Build menu from discovered commands
        var commandList = new List<IConsoleCommand>(commands);

        if (commandList.Count == 0)
        {
            console.WriteLine("No console commands discovered.");
            return;
        }

        while (true)
        {
            console.WriteLine("");
            console.WriteLine("== Application Menu ==");

            for (int i = 0; i < commandList.Count; i++)
            {
                console.WriteLine($"{i + 1}) {commandList[i].MenuLabel}");
            }

            console.WriteLine("0) Exit");

            string choice = console.AskInput("Choose an option: ");
            if (string.IsNullOrWhiteSpace(choice))
                continue;

            choice = choice.Trim();
            if (choice == "0")
                break;

            if (int.TryParse(choice, out int idx))
            {
                idx -= 1; // make zero-based
                if (idx >= 0 && idx < commandList.Count)
                {
                    try
                    {
                        commandList[idx].Execute();
                    }
                    catch (Exception ex)
                    {
                        console.WriteLine($"Error executing command: {ex.Message}");
                    }
                }
                else
                {
                    console.WriteLine("Invalid option. Try again.");
                }
            }
            else
            {
                console.WriteLine("Invalid input. Enter a number.");
            }
        }
    }
}
