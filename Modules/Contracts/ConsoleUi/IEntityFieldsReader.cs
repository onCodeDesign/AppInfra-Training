namespace Contracts.ConsoleUi;

public interface IEntityFieldsReader<T>
{
    IEnumerable<string> GetFields();
    void SetFieldValue(string fieldName, string value);
    T GetEntity();
}
