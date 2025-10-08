using AppBoot.SystemEx;
using DataAccess.Exceptions;
using Microsoft.Data.SqlClient;

namespace DataAccess.ExceptionHandling;

internal class SqlExceptionHandler : IExceptionHandler
{
    private readonly IExceptionHandler successor;

    public SqlExceptionHandler(IExceptionHandler successor)
    {
        this.successor = successor;
    }

    public void Handle(Exception exception)
    {
        var sqlException = exception.FirstInner<SqlException>();
        if (sqlException != null)
        {
            throw (object)sqlException.Number switch
            {
                242 => new DateTimeRangeRepositoryViolationException(sqlException),
                547 => new DeleteConstraintRepositoryViolationException(sqlException),
                1205 => new DeadlockVictimRepositoryViolationException(sqlException),
                2601 or 2627 => new UniqueConstraintRepositoryViolationException(sqlException),
                _ => new RepositoryViolationException(sqlException),
            };
        }

        successor.Handle(exception);
    }
}