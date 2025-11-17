using AppBoot.DependencyInjection;
using Contracts.Notifications;
using DataAccess;
using ProductsManagement.DataModel.SalesLT;

namespace ProductsManagement.Services.ModelInterceptors;

[Service(typeof(IEntityInterceptor<ProductCategory>))]
internal class ProductCategoryNotificationInterceptor : EntityInterceptor<ProductCategory>
{
    private readonly INotificationService notificationService;

    public ProductCategoryNotificationInterceptor(INotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    public override void OnSave(IEntityEntry<ProductCategory> entry, IUnitOfWork unitOfWork)
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

    public override void OnDelete(IEntityEntry<ProductCategory> entry, IUnitOfWork unitOfWork)
    {
        notificationService.NotifyDeleted(entry.Entity);
    }

    public override void OnLoad(IEntityEntry<ProductCategory> entry, IRepository repository)
    {
    }
}
