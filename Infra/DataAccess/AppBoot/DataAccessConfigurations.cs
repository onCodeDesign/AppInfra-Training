using Microsoft.Extensions.DependencyInjection;
using AppBoot.DependencyInjection;


namespace DataAccess.AppBoot;

public static class DataAccessConfigurations
{
    public static ConventionRegistrationBehavior DefaultRegistrationConventions
    {
        get { return GetDefaultConventions(); }
    }

    private static ConventionRegistrationBehavior GetDefaultConventions()
    {
        var conventions = new ConventionRegistrationBehavior();

        conventions.ForType<Repository>().Export(b => b.AsContractType<IRepository>());
        conventions.ForType<InterceptorsResolver>().Export(b => b.AsContractType<IInterceptorsResolver>().WithServiceLifetime(ServiceLifetime.Singleton));

        return conventions;
    }
}