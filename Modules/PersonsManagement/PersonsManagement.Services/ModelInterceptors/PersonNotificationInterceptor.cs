using AppBoot.DependencyInjection;
using Contracts.Notifications;
using DataAccess;
using PersonsManagement.DataModel.Persons;

namespace PersonsManagement.Services.ModelInterceptors;

[Service(typeof(IEntityInterceptor<Person>))]
internal class PersonNotificationInterceptor : EntityInterceptor<Person>
{
    private readonly INotificationService notificationService;

    public PersonNotificationInterceptor(INotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    public override void OnSave(IEntityEntry<Person> entry, IUnitOfWork unitOfWork)
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

    public override void OnDelete(IEntityEntry<Person> entry, IUnitOfWork unitOfWork)
    {
        notificationService.NotifyDeleted(entry.Entity);
    }

    public override void OnLoad(IEntityEntry<Person> entry, IRepository repository)
    {
    }
}
