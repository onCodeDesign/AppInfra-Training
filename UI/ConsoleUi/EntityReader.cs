using System.Reflection;
using AppBoot.DependencyInjection;
using Contracts.ConsoleUi;

namespace ConsoleUi;

[Service(typeof(IEntityReader))]
internal class EntityReader(IConsole console) : IEntityReader
{
    public IEntityFieldsReader<T> BeginEntityRead<T>() where T : new()
    {
        return new EntityFieldsReader<T>(console);
    }
}

internal class EntityFieldsReader<T> : IEntityFieldsReader<T> where T : new()
{
    private readonly IConsole console;
    private readonly T entity;
    private readonly Dictionary<string, PropertyInfo> properties;

    public EntityFieldsReader(IConsole console)
    {
        this.console = console;
        this.entity = new T();
        
        // Get writable properties
        this.properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite && p.CanRead)
            .Where(p => IsSimpleType(p.PropertyType))
            .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<string> GetFields()
    {
        return properties.Keys.OrderBy(k => k);
    }

    public void SetFieldValue(string fieldName, string value)
    {
        if (!properties.TryGetValue(fieldName, out var property))
        {
            console.WriteLine($"Warning: Property '{fieldName}' not found.");
            return;
        }

        try
        {
            object? convertedValue = ConvertValue(value, property.PropertyType);
            property.SetValue(entity, convertedValue);
        }
        catch (Exception ex)
        {
            console.WriteLine($"Error setting {fieldName}: {ex.Message}");
        }
    }

    public T GetEntity()
    {
        return entity;
    }

    private static bool IsSimpleType(Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
        
        return underlyingType.IsPrimitive
            || underlyingType.IsEnum
            || underlyingType == typeof(string)
            || underlyingType == typeof(decimal)
            || underlyingType == typeof(DateTime)
            || underlyingType == typeof(DateTimeOffset)
            || underlyingType == typeof(TimeSpan)
            || underlyingType == typeof(Guid);
    }

    private static object? ConvertValue(string value, Type targetType)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            if (Nullable.GetUnderlyingType(targetType) != null)
                return null;
            
            if (!targetType.IsValueType)
                return null;
        }

        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (underlyingType == typeof(string))
            return value;

        if (underlyingType == typeof(Guid))
            return Guid.Parse(value);

        if (underlyingType == typeof(DateTime))
            return DateTime.Parse(value);

        if (underlyingType.IsEnum)
            return Enum.Parse(underlyingType, value, true);

        return Convert.ChangeType(value, underlyingType);
    }
}
