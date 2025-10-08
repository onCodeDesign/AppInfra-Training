using System.ComponentModel.DataAnnotations;
using AppBoot.SystemEx;
using DataAccess.Exceptions;

namespace DataAccess.ExceptionHandling;

class DbEntityValidationExceptionHandler : IExceptionHandler
{
    private readonly IExceptionHandler successor;

    public DbEntityValidationExceptionHandler(IExceptionHandler successor)
    {
        this.successor = successor;
    }

    public void Handle(Exception exception)
    {
        var validationException = exception.FirstInner<ValidationException>();
        if (validationException != null)
        {
            throw new DataValidationException(validationException.Message, validationException);
        }

        successor.Handle(exception);
    }
}