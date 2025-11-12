using AppBoot.DependencyInjection;
using DataAccess;
using Sales.DataModel.SalesLT;

namespace Sales.Services.ModelInterceptors;

[Service(typeof(IEntityInterceptor<SalesOrderHeader>))]
class SalesOrderValidationInterceptor : IEntityInterceptor<SalesOrderHeader>
{
    public void OnSave(IEntityEntry<SalesOrderHeader> entry, IUnitOfWork unitOfWork)
    {
        if (IsAddedOrModified(entry))
        {
            var order = entry.Entity;
            if (string.IsNullOrEmpty(order.AccountNumber))
                throw new InvalidOrderException();

            HashSet<string> modifiedNames = entry
                .GetProperties()
                .Where(n => entry.Property(n).IsModified)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (modifiedNames.Overlaps(new[] {
                    nameof(SalesOrderHeader.TaxAmt),
                    nameof(SalesOrderHeader.SubTotal),
                    nameof(SalesOrderHeader.Freight),
                    nameof(SalesOrderHeader.TotalDue)
                }))
            {
                if (order.TaxAmt < order.SubTotal + order.TaxAmt + order.Freight)
                    throw new InvalidOrderException();
            }
        }
    }

    private static bool IsAddedOrModified(IEntityEntry<SalesOrderHeader> entry)
    {
        return entry.State.HasFlag(EntityEntryState.Added) || entry.State.HasFlag(EntityEntryState.Modified);
    }

    public void OnSave(IEntityEntry entry, IUnitOfWork unitOfWork)
    {
        this.OnSave(entry.Convert<SalesOrderHeader>(), unitOfWork);
    }

    public void OnLoad(IEntityEntry entry, IRepository repository)
    {
    }

    public void OnLoad(IEntityEntry<SalesOrderHeader> entry, IRepository repository)
    {
    }

    public void OnDelete(IEntityEntry entry, IUnitOfWork unitOfWork)
    {
    }

    public void OnDelete(IEntityEntry<SalesOrderHeader> entry, IUnitOfWork unitOfWork)
    {
    }
}