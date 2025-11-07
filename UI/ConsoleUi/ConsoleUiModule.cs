using AppBoot;
using AppBoot.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System;

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
            // leave some space before the menu
            console.WriteLine("");
            console.WriteLine("");

            // Draw menu in a distinct color
            var previousColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                console.WriteLine("== Application Menu ==");

                for (int i = 0; i < commandList.Count; i++)
                {
                    console.WriteLine($"{i + 1}) {commandList[i].MenuLabel}");
                }

                console.WriteLine("0) Exit");
            }
            finally
            {
                Console.ForegroundColor = previousColor;
            }

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
                    // leave blank line between menu and command output
                    console.WriteLine("");

                    var prev = Console.ForegroundColor;
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        try
                        {
                            commandList[idx].Execute();
                        }
                        catch (Exception ex)
                        {
                            console.WriteLine($"Error executing command: {ex.Message}");
                        }
                    }
                    finally
                    {
                        Console.ForegroundColor = prev;
                    }

                    // leave a blank line after command execution
                    console.WriteLine("");
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
