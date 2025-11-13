using AppBoot.DependencyInjection;
using Contracts.Notifications;
using DataAccess;
using Sales.DataModel.SalesLT;

namespace Sales.Services.ModelInterceptors;

[Service(typeof(IEntityInterceptor<SalesOrderHeader>))]
class SalesOrderNotificationInterceptor(INotificationService notificationService) : EntityInterceptor<SalesOrderHeader>
{
    public override void OnSave(IEntityEntry<SalesOrderHeader> entry, IUnitOfWork unitOfWork)
    {
        if (entry.State.HasFlag(EntityEntryState.Added))
        {
            notificationService.NotifyNew(entry.Entity);
        }
        else if (entry.State.HasFlag(EntityEntryState.Modified))
        {
            notificationService.NotifyChanged(entry.Entity);
        }
    }

    public override void OnDelete(IEntityEntry<SalesOrderHeader> entry, IUnitOfWork unitOfWork)
    {
        notificationService.NotifyDeleted(entry.Entity);
    }

    public override void OnLoad(IEntityEntry<SalesOrderHeader> entry, IRepository repository)
    {
    }
}
