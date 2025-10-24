using AppBoot;
using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ConsoleUi;

[Service(typeof(IModule))]
public class ConsoleUiModule : IModule
{
    private readonly IDictionary<string, IConsoleCommand> commands;
    private readonly IConsole console;

    public ConsoleUiModule(IEnumerable<IConsoleCommand> commands, IConsole console)
    {
        this.console = console;
        int i = 1;
        this.commands = new Dictionary<string, IConsoleCommand>();
        foreach (var command in commands)
        {
            this.commands.Add(i.ToString(), command);
            i++;
        }
    }

    public string Name => "Console UI Module";
    public void Initialize(IHost host)
    {
        console.WriteLine("Console UI Module initialized.");
        console.WriteLine("");

        string commandKey = string.Empty;
       
        while (commandKey!= "0")
        {
           commandKey = PrintMenu();

            console.WriteLine($"Executing command: {commandKey}");
            console.WriteLine("");

            if (commands.TryGetValue(commandKey, out var command))
            {
                try
                {
                    command.Execute();
                    console.WriteLine("");
                    console.WriteLine($"Command {commandKey} has been executed.");
                }
                catch (Exception ex)
                {
                    console.WriteLine($"Error executing command {commandKey}: {ex.Message}");
                }
            }
            else if (commandKey != "0")
            {
                console.WriteLine($"Invalid command key: {commandKey}");
            }
        }

    }

    private string PrintMenu()
    {
        var color = Console.ForegroundColor;
        console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Cyan;
        try
        {
            console.WriteLine("=== Main Menu ===");
            console.WriteLine("Available commands:");
            foreach (var command in commands)
            {
                console.WriteLine($"{command.Key} - {command.Value.MenuLabel}");
            }

            console.WriteLine("0 - Exit");
            console.WriteLine("");
            return console.AskInput("Enter command key: ");
        }
        finally
        {
            Console.ForegroundColor = color;
        }
    }
}