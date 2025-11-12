using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;
using AppBoot.SystemEx.Priority;

namespace Sales.ConsoleCommands
{
    [Service(typeof(IConsoleCommand))]
    [Priority(5)]
    internal sealed class TestSalesCommand : IConsoleCommand
    {
        private readonly IConsole console;

        public TestSalesCommand(IConsole console)
        {
            this.console = console;
        }

        public string MenuLabel => "Sales: Show top customers";

        public void Execute()
        {
            console.WriteLine("Sales: top customers (test command)");
        }
    }
}
