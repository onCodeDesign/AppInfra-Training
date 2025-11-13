using AppBoot.DependencyInjection;
using DataAccess;
using Sales.DataModel;

namespace Sales.Services.ModelInterceptors;

[Service(typeof(IEntityInterceptor))]
internal sealed class AuditableInterceptor : GlobalEntityInterceptor<IAuditable>
{
    public override void OnSave(IEntityEntry<IAuditable> entry, IUnitOfWork unitOfWork)
    {
        if (entry.State.HasFlag(EntityEntryState.Added) || entry.State.HasFlag(EntityEntryState.Modified))
        {
            entry.Entity.ModifiedDate = DateTime.UtcNow;
        }
    }

    public override void OnDelete(IEntityEntry<IAuditable> entry, IUnitOfWork unitOfWork)
    {
    }

    public override void OnLoad(IEntityEntry<IAuditable> entry, IRepository repository)
    {
    }
}
