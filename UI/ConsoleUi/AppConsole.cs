using AppBoot.DependencyInjection;
using AppBoot.SystemEx;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleUi;

[Service(typeof(IConsole), ServiceLifetime.Singleton)]
class AppConsole : IConsole
{
    public string AskInput(string message)
    {
        Console.WriteLine();
        Console.WriteLine(message);

        return Console.ReadLine() ?? string.Empty;
    }

    public string ReadLine()
    {
        return Console.ReadLine() ?? string.Empty;
    }

    public ConsoleKeyInfo ReadKey()
    {
        return Console.ReadKey();
    }

    public void WriteEntity<T>(T entity)
    {
        Console.WriteLine();
        Console.WriteLine($"--------------- {typeof(T).Name} ----------------");
        if (entity != null)
        {
            var properties = ReflectionExtensions.GetEditableSimpleProperties(entity);
            foreach (var propertyInfo in properties)
            {
                Console.Write($"{propertyInfo.Name}: ");
                Console.WriteLine(propertyInfo.GetValue(entity));
            }
        }
        else
        {
            Console.WriteLine("Entity is NULL");
        }
        Console.WriteLine("-----------------------------------------------------");
    }

    public void WriteLine(string line)
    {
        Console.WriteLine(line);
    }

    public void Clear()
    {
        Console.Clear();
    }
}