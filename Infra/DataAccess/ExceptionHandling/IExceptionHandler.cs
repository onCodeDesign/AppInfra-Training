namespace DataAccess.ExceptionHandling;

public interface IExceptionHandler
{
    void Handle(Exception exception);
}