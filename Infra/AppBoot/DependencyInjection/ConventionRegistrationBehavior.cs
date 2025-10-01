using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AppBoot.DependencyInjection;

public class ConventionRegistrationBehavior : IRegistrationBehavior
{
    private readonly IList<ServiceBuilder> builders = new List<ServiceBuilder>();

    public IEnumerable<ServiceDescriptor> GetServicesFrom(Type type)
    {
        var services = builders.SelectMany(x => x.GetServicesFrom(type));
        return services;
    }

    public ServiceBuilder ForType(Type type)
    {
        var builder = CreateServiceBuilder(x => x == type);
        builders.Add(builder);
        return builder;
    }

    [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Convenience call")]
    public ServiceBuilder ForType<T>()
    {
        return ForType(typeof(T));
    }

    public ServiceBuilder ForTypesDerivedFrom(Type type)
    {
        var builder = CreateServiceBuilder(x => type.IsAssignableFrom(x));
        builders.Add(builder);
        return builder;
    }

    [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Convenience call")]
    public ServiceBuilder ForTypesDerivedFrom<T>()
    {
        return ForTypesDerivedFrom(typeof(T));
    }

    public ServiceBuilder ForTypesMatching(Predicate<Type> typeFilter)
    {
        var builder = CreateServiceBuilder(typeFilter);
        builders.Add(builder);
        return builder;
    }


    private static ServiceBuilder CreateServiceBuilder(Predicate<Type> typeFilter)
    {
        return new ServiceBuilder(typeFilter);
    }
}
