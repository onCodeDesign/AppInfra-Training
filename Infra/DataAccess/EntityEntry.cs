using Microsoft.EntityFrameworkCore;
using EFCore = Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess;

sealed class EntityEntry<T> : IEntityEntry<T>
    where T : class
{
    private readonly EFCore.EntityEntry efEntry;

    public EntityEntry(EFCore.EntityEntry entry)
    {
        this.efEntry = entry;
        this.Entity = (T)efEntry.Entity;
    }

    public T Entity { get; }

    public EntityEntryState State
    {
        get => (EntityEntryState)efEntry.State;
        set => efEntry.State = (EntityState)value;
    }

    public object GetOriginalValue(string propertyName)
    {
        return efEntry.OriginalValues[propertyName];
    }

    public object GetCurrentValue(string propertyName)
    {
        return efEntry.CurrentValues[propertyName];
    }

    public void SetOriginalValue(string propertyName, object value)
    {
        var originalProperty = efEntry.OriginalValues.Properties.FirstOrDefault(p => p.Name == propertyName);
        if (originalProperty != null)
        {
            efEntry.OriginalValues[propertyName] = value;
        }
    }

    public void Reload()
    {
        efEntry.Reload();
    }

    public IEnumerable<string> GetProperties()
    {
        return efEntry.CurrentValues.Properties.Select(p => p.Name);
    }

    public IPropertyEntry Property(string name)
    {
        return new PropertyEntry(efEntry.Property(name));
    }

    private bool Equals(EntityEntry<T> other)
    {
        return Equals(efEntry, other.efEntry);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is EntityEntry<T> && Equals((EntityEntry<T>)obj);
    }

    public override int GetHashCode()
    {
        return (efEntry != null ? efEntry.GetHashCode() : 0);
    }
}

sealed class EntityEntry : IEntityEntry
{
    private readonly EFCore.EntityEntry efEntry;

    public EntityEntry(EFCore.EntityEntry entry)
    {
        this.efEntry = entry;
    }

    public object Entity
    {
        get { return this.efEntry.Entity; }
    }

    public EntityEntryState State
    {
        get { return (EntityEntryState)efEntry.State; }
        set { efEntry.State = (EntityState)value; }
    }

    public object GetOriginalValue(string propertyName)
    {
        return efEntry.OriginalValues[propertyName];
    }

    public object GetCurrentValue(string propertyName)
    {
        return efEntry.CurrentValues[propertyName];
    }

    public IEntityEntry<T> Convert<T>() where T : class
    {
        return new EntityEntry<T>(efEntry);
    }

    public void SetOriginalValue(string propertyName, object value)
    {
        var originalProperty = efEntry.OriginalValues.Properties.FirstOrDefault(p => p.Name == propertyName);
        if (originalProperty != null)
        {
            efEntry.OriginalValues[propertyName] = value;
        }
    }

    public void Reload()
    {
        efEntry.Reload();
    }

    public IEnumerable<string> GetProperties()
    {
        return efEntry.CurrentValues.Properties.Select(p => p.Name);
    }

    public IPropertyEntry Property(string name)
    {
        return new PropertyEntry(efEntry.Property(name));
    }

    private bool Equals(EntityEntry other)
    {
        return Equals(efEntry, other.efEntry);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is EntityEntry && Equals((EntityEntry)obj);
    }

    public override int GetHashCode()
    {
        return (efEntry != null ? efEntry.GetHashCode() : 0);
    }

}

sealed class PropertyEntry : IPropertyEntry
{
    private readonly EFCore.PropertyEntry entry;

    public PropertyEntry(EFCore.PropertyEntry entry)
    {
        this.entry = entry;
    }

    public string Name
    {
        get { return entry.GetType().Name; }
    }

    public object CurentValue
    {
        get { return entry.CurrentValue; }
        set { entry.CurrentValue = value; }
    }
    public object OriginalValue
    {
        get { return entry.OriginalValue; }
        set { entry.OriginalValue = value; }
    }
    public bool IsModified
    {
        get { return entry.IsModified; }
        set { entry.IsModified = value; }
    }
}