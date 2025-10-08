namespace DataAccess;

public interface IInterceptorsResolver
{
    IEnumerable<IEntityInterceptor> GetGlobalInterceptors();
    IEnumerable<IEntityInterceptor> GetEntityInterceptors(Type entityType);
}