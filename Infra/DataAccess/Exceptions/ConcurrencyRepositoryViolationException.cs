using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Exceptions;

[Serializable]
public class ConcurrencyRepositoryViolationException : RepositoryViolationException
{
    private const string RepositoryEntityKey = "Entity";

    public object Entity { get; set; }

    public ConcurrencyRepositoryViolationException()
    {
    }

    public ConcurrencyRepositoryViolationException(string errorMessage)
        : base(errorMessage)
    {
    }

    public ConcurrencyRepositoryViolationException(DbUpdateException exception)
        : this(string.Empty, exception)
    {

    }

    public ConcurrencyRepositoryViolationException(string message, DbUpdateException exception)
        : base(message, exception)
    {
        if (!exception.Entries.Any())
        {
            var entry = exception.Entries?.FirstOrDefault();
            if (entry != null)
                this.Entity = entry.Entity;
        }
    }

    protected ConcurrencyRepositoryViolationException(SerializationInfo info, StreamingContext context)
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