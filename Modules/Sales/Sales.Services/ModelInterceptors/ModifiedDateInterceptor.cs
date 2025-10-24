using AppBoot.DependencyInjection;
using DataAccess;
using Sales.DataModel;

namespace Sales.Services.ModelInterceptors;

[Service(typeof(IEntityInterceptor))]
class ModifiedDateInterceptor : IEntityInterceptor
{
    public void OnLoad(IEntityEntry entry, IRepository repository)
    {
    }

    public void OnSave(IEntityEntry entry, IUnitOfWork unitOfWork)
    {
        if (entry.Entity is IAuditable auditable)
        {
            auditable.ModifiedDate = DateTime.UtcNow;
        }
    }

    public void OnDelete(IEntityEntry entry, IUnitOfWork unitOfWork)
    {
    }
}