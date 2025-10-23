using AppBoot.DependencyInjection;
using ConsoleUi;
using Contracts.ConsoleUi;

namespace Notifications.Services;

[Service(typeof(IConsoleCommand))]
class MyDemoCommand(IConsole console) : IConsoleCommand
{
    public void Execute()
    {
        console.WriteLine("This is a demo from Notifications.Services.MyDemoCommand.");
    }

    public string MenuLabel => "My demo command";
}