namespace DataAccess;

public interface IDbContextFactory
{
    IDbContextWrapper CreateContext();
}