using System.Diagnostics;

namespace DataAccess;

public abstract class EntityInterceptor<T> : IEntityInterceptor<T>
    where T : class
{
    public abstract void OnLoad(IEntityEntry<T> entry, IRepository repository);
    public abstract void OnSave(IEntityEntry<T> entry, IUnitOfWork repository);
    public abstract void OnDelete(IEntityEntry<T> entry, IUnitOfWork repository);

    void IEntityInterceptor.OnLoad(IEntityEntry entry, IRepository repository)
    {
        if (entry.Entity is T)
            this.OnLoad(entry.Convert<T>(), repository);
        else
            Debug.Assert(false, "Entity should be of type " + typeof(T).FullName);
    }

    void IEntityInterceptor.OnSave(IEntityEntry entry, IUnitOfWork repository)
    {
        if (entry.Entity is T)
            this.OnSave(entry.Convert<T>(), repository);
        else
            Debug.Assert(false, "Entity should be of type " + typeof(T).FullName);
    }

    void IEntityInterceptor.OnDelete(IEntityEntry entry, IUnitOfWork repository)
    {
        if (entry.Entity is T)
            this.OnDelete(entry.Convert<T>(), repository);
        else
            Debug.Assert(false, "Entity should be of type " + typeof(T).FullName);
    }
}