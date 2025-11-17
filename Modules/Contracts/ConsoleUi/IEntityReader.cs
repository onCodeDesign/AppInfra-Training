namespace Contracts.ConsoleUi;

public interface IEntityReader
{
    IEntityFieldsReader<T> BeginEntityRead<T>() where T : new();
}
