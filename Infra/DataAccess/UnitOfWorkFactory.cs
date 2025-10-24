using AppBoot.DependencyInjection;

namespace DataAccess;

[Service(typeof(IUnitOfWorkFactory))]
public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory contextFactory;
    private readonly IInterceptorsResolver interceptorsResolver;

    public UnitOfWorkFactory(IDbContextFactory contextFactory, IInterceptorsResolver interceptorsResolver)
    {
        this.contextFactory = contextFactory;
        this.interceptorsResolver = interceptorsResolver;
    }

    public IUnitOfWork CreateUnitOfWork()
    {
        return new UnitOfWork(contextFactory, interceptorsResolver);
    }
}