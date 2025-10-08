using AppBoot.DependencyInjection;
using Contracts.Notifications;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Json;

namespace Notifications.Services;

[Service(typeof (IStateChangeSubscriber<>), ServiceLifetime.Singleton)]
class FileLoggerStateChangeSubscriber<T> : IStateChangeSubscriber<T>
{
    private readonly object lockObj = new();
    private readonly string logFilePath;
    private readonly string dashboardLogPath;

    public FileLoggerStateChangeSubscriber()
    {
        string logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        Directory.CreateDirectory(logsDirectory);
        logFilePath = Path.Combine(logsDirectory, $"{typeof(T).Name}_log_{DateTime.Now:yyyyMMdd}.txt");
        
        // Create monitoring dashboard directory
        string dashboardDirectory = Path.Combine(logsDirectory, "dashboard");
        Directory.CreateDirectory(dashboardDirectory);
        dashboardLogPath = Path.Combine(dashboardDirectory, $"{typeof(T).Name}_dashboard_{DateTime.Now:yyyyMMdd}.txt");
    }

    public void NewItem(T item)
    {
        lock (lockObj) 
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string itemJson = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });
                string logEntry = $"[{timestamp}] NEW ITEM: {itemJson}{Environment.NewLine}";
                
                File.AppendAllText(logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                // Log the exception to a fallback location if the main logging fails
                string errorMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ERROR logging new item: {ex.Message}{Environment.NewLine}";
                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logging_errors.txt"), errorMessage);
            }
        }
    }

    public void NotifyDeleted(T item)
    {
        lock (lockObj)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string itemJson = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });
                string dashboardEntry = $"[{timestamp}] DELETED ITEM: {itemJson}{Environment.NewLine}";
                
                File.AppendAllText(dashboardLogPath, dashboardEntry);
            }
            catch (Exception ex)
            {
                // Log the exception to a fallback location if the dashboard logging fails
                string errorMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ERROR logging deleted item: {ex.Message}{Environment.NewLine}";
                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_errors.txt"), errorMessage);
            }
        }
    }

    public void NotifyChanged(T item)
    {   
        lock (lockObj)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string itemJson = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });
                string dashboardEntry = $"[{timestamp}] CHANGED ITEM: {itemJson}{Environment.NewLine}";
                
                File.AppendAllText(dashboardLogPath, dashboardEntry);
            }
            catch (Exception ex)
            {
                // Log the exception to a fallback location if the dashboard logging fails
                string errorMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ERROR logging changed item: {ex.Message}{Environment.NewLine}";
                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_errors.txt"), errorMessage);
            }
        }
    }
}