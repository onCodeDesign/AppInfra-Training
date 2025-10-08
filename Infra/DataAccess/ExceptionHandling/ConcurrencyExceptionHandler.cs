using AppBoot.SystemEx;
using DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.ExceptionHandling;

internal class ConcurrencyExceptionHandler : IExceptionHandler
{
    private readonly IExceptionHandler successor;

    public ConcurrencyExceptionHandler(IExceptionHandler successor)
    {
        this.successor = successor;
    }

    public void Handle(Exception exception)
    {
        var concurrencyException = exception.FirstInner<DbUpdateConcurrencyException>();
        if (concurrencyException != null)
        {
            throw new ConcurrencyRepositoryViolationException(concurrencyException);
        }

        successor.Handle(exception);
    }
}