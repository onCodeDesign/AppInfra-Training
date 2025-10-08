using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Exceptions;

[Serializable]
public class RepositoryUpdateException : RepositoryViolationException
{
    private const string RepositoryEntityKey = "Entity";
    public object Entity { get; set; }

    public RepositoryUpdateException()
    {
    }

    public RepositoryUpdateException(string errorMessage)
        : base(errorMessage)
    {
    }

    public RepositoryUpdateException(DbUpdateException exception)
        : this(string.Empty, exception)
    {
    }

    public RepositoryUpdateException(string message, DbUpdateException exception)
        : base(message, exception)
    {
        if (!exception.Entries.Any())
        {
            var entry = exception.Entries.FirstOrDefault();
            if (entry != null)
                this.Entity = entry.Entity;
        }
    }

    protected RepositoryUpdateException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        this.Entity = info.GetValue(RepositoryEntityKey, typeof(object));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(RepositoryEntityKey, this.Entity, typeof(string));
    }
}