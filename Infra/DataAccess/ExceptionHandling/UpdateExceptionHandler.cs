using AppBoot.SystemEx;
using DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.ExceptionHandling;

internal class UpdateExceptionHandler : IExceptionHandler
{
    private readonly IExceptionHandler successor;

    public UpdateExceptionHandler(IExceptionHandler successor)
    {
        this.successor = successor;
    }

    public void Handle(Exception exception)
    {
        var updateException = exception.FirstInner<DbUpdateException>();
        if (updateException != null)
        {
            throw new RepositoryUpdateException(updateException);
        }

        successor.Handle(exception);
    }
}