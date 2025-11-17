using AppBoot;
using AppBoot.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System;
using Contracts.ConsoleUi;
using System.Linq;
using System.Reflection;
using AppBoot.SystemEx.Priority;

namespace ConsoleUi;

[Priority(Priorities.VeryLow)]
[Service(typeof(IModule))]
internal sealed class ConsoleUiModule(IConsole console, IEnumerable<IConsoleCommand> commands) : IModule
{
    private record MenuEntry(IConsoleCommand Command, string Module, int Priority);

    public void Initialize(IHost host)
    {
        List<IConsoleCommand> commandList = new List<IConsoleCommand>(commands);

        if (commandList.Count == 0)
        {
            console.WriteLine("No console commands discovered.");
            return;
        }

        var sortedCommands = commandList
            .Select(c =>
            {
                var t = c.GetType();
                var asmName = t.Assembly.GetName().Name ?? string.Empty;
                var module = asmName.Split('.').FirstOrDefault() ?? asmName;
                var prAttr = t.GetCustomAttribute<PriorityAttribute>();
                var priority = prAttr?.Value ?? 0;
                return new MenuEntry(c, module, priority);
            })
            .OrderBy(ci => ci.Module)
            .ThenBy(ci => ci.Priority)
            .ToList();

        while (true)
        {
            DrawMenu(sortedCommands);

            string choice = console.AskInput("Choose an option: ");
            if (string.IsNullOrWhiteSpace(choice))
                continue;

            choice = choice.Trim();
            if (choice == "0")
                break;

            if (int.TryParse(choice, out int idx))
            {
                idx -= 1;
                if (idx >= 0 && idx < sortedCommands.Count)
                {
                    console.WriteLine("");

                    var prev = Console.ForegroundColor;
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        try
                        {
                            sortedCommands[idx].Command.Execute();
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

    private void DrawMenu(List<MenuEntry> sorted)
    {
        console.WriteLine(""); console.WriteLine("");

        var previousColor = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            console.WriteLine("== Application Menu ==");

            int index = 1;
            string previousModule = null;
            foreach (var ci in sorted)
            {
                if (!string.Equals(previousModule, ci.Module, StringComparison.Ordinal))
                {
                    var prev = Console.ForegroundColor;
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        console.WriteLine($"-- {ci.Module} --");
                    }
                    finally
                    {
                        Console.ForegroundColor = prev;
                    }

                    previousModule = ci.Module;
                }

                console.WriteLine($"\t{index}) {ci.Command.MenuLabel}");
                index++;
            }

            console.WriteLine("(0) Exit");
        }
        finally
        {
            Console.ForegroundColor = previousColor;
        }
    }
}
