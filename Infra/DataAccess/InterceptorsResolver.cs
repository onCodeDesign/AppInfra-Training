using AppBoot.DependencyInjection;
using AppBoot.SystemEx.Priority;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

[Service(typeof(IInterceptorsResolver), ServiceLifetime.Singleton)]
public class InterceptorsResolver : IInterceptorsResolver
{
    private readonly IServiceProvider serviceProvider;

    private static readonly Type interceptorGenericType = typeof(IEntityInterceptor<>);

    public InterceptorsResolver(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IEnumerable<IEntityInterceptor> GetGlobalInterceptors()
    {
        return serviceProvider.GetServices<IEntityInterceptor>().OrderByPriority().DistinctBy(e => e.GetType());
    }

    public IEnumerable<IEntityInterceptor> GetEntityInterceptors(Type entityType)
    {
        Type interceptorType = interceptorGenericType.MakeGenericType(entityType);
        return serviceProvider.GetServices(interceptorType).Cast<IEntityInterceptor>();
    }
}