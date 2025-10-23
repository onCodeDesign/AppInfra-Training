using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.ConsoleUi;

public interface IConsoleCommand
{
    void Execute();
    string MenuLabel { get; }
}