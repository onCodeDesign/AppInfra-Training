using DataAccess.Exceptions;

namespace DataAccess.ExceptionHandling;

internal class DefaultExceptionHandler : IExceptionHandler
{
    public void Handle(Exception exception)
    {
        throw new RepositoryViolationException(exception);
    }
}