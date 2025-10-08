using System.Diagnostics;
using System.Linq.Expressions;
using AppBoot.DependencyInjection;
using Contracts.Sales.CustomerOrders;
using DataAccess;
using Sales.DataModel.SalesLT;

namespace Sales.Services;

[Service(typeof(ICustomerOrdersService))]
class CustomerOrdersService : ICustomerOrdersService
{
    private readonly IRepository rep;

    public CustomerOrdersService(IRepository rep)
    {
        this.rep = rep;
    }

    public IEnumerable<CustomerData> GetCustomersWithOrders()
    {
        return GetCustomersBy(c => true);
    }

    public IEnumerable<CustomerData> GetCustomersWithOrdersByName(string nameContains, string nameStartsWith)
    {
        Debug.Assert(!(string.IsNullOrEmpty(nameContains) && string.IsNullOrEmpty(nameStartsWith)));

        Expression<Func<Customer, bool>> filter;
        if (!string.IsNullOrEmpty(nameStartsWith) && !string.IsNullOrEmpty(nameContains))
            filter = c => c.CompanyName.StartsWith(nameStartsWith) && c.CompanyName.Contains(nameContains);
        else if (!string.IsNullOrEmpty(nameStartsWith))
            filter = c => c.CompanyName.StartsWith(nameStartsWith);
        else
            filter = c => c.CompanyName.Contains(nameContains);

        return GetCustomersBy(filter);
    }

    public void CancelAllOrdersForCustomer(string customerName)
    {
        using (var uof = rep.CreateUnitOfWork())
        {
            var customerOrders = uof.GetEntities<SalesOrderHeader>()
                .Where(o => o.Customer.LastName == customerName);

            foreach (var salesOrderHeader in customerOrders)
            {
                salesOrderHeader.Status = 1;
            }

            uof.SaveChanges();
        }
    }

    private IEnumerable<CustomerData> GetCustomersBy(Expression<Func<Customer, bool>> filter)
    {
        var query = rep.GetEntities<Customer>()
            .Where(c => c.SalesOrderHeaders.Any() && c.SalesPerson != null)
            .Where(filter)
            .OrderBy(c => c.SalesPerson)
            .Select(c => new CustomerData
            {
                Id = c.CustomerID,
                SalesPerson = c.SalesPerson,
                Name = c.CompanyName
            });

        return query.AsEnumerable();
    }
}