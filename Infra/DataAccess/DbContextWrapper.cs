using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public sealed class DbContextWrapper : IDbContextWrapper
{
    public DbContextWrapper(DbContext context)
    {
        Context = context;
    }

    public DbContext Context { get; private set; }

    public event EntityLoadedEventHandler? EntityLoaded;

    public void Dispose()
    {
        Context.Dispose();
    }
}