namespace ConsoleUi;

public interface IConsoleCommand
{
    void Execute();
    string MenuLabel { get; }
}
