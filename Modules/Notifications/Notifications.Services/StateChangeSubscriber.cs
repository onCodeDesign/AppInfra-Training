using System.Text.Json;
using AppBoot.DependencyInjection;
using Contracts.Notifications;
using Microsoft.Extensions.Logging;

namespace Notifications.Services;

[Service(typeof(IStateChangeSubscriber<>))]
class StateChangeSubscriber<T> : IStateChangeSubscriber<T>
{
    private readonly ILogger<StateChangeSubscriber<T>> logger;
    private readonly string logFilePath;

    public StateChangeSubscriber(ILogger<StateChangeSubscriber<T>> logger)
    {
        this.logger = logger;
        
        var logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        Directory.CreateDirectory(logsDirectory);
        
        logFilePath = Path.Combine(logsDirectory, "EntityChanges.log");
    }

    public void NewItem(T item)
    {
        LogToFile("CREATED", item);
    }

    public void NotifyDeleted(T item)
    {
        LogToFile("DELETED", item);
    }

    public void NotifyChanged(T item)
    {
        LogToFile("CHANGED", item);
    }

    private void LogToFile(string action, T item)
    {
        try
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var entityType = typeof(T).Name;
            var entityData = SerializeEntity(item);
            
            var logEntry = $"[{timestamp}] {action} - {entityType}: {entityData}";
            
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            
            logger.LogInformation("Entity {Action}: {EntityType}", action, entityType);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to log entity change to file for type {EntityType}", typeof(T).Name);
        }
    }

    private string SerializeEntity(T item)
    {
        try
        {
            return JsonSerializer.Serialize(item, new JsonSerializerOptions 
            { 
                WriteIndented = false,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });
        }
        catch
        {
            return item?.ToString() ?? "null";
        }
    }
}