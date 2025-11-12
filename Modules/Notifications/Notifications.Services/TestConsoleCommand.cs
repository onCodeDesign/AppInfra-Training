using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;
using AppBoot.SystemEx.Priority;

namespace Notifications.Services
{
    [Service(typeof(IConsoleCommand))]
    [Priority(10)]
    internal sealed class TestConsoleCommand : IConsoleCommand
    {
        private readonly IConsole console;

        public TestConsoleCommand(IConsole console)
        {
            this.console = console;
        }

        public string MenuLabel => "Notifications: Show unread";

        public void Execute()
        {
            console.WriteLine("Notifications: no unread messages (test command)");
        }
    }
}
