using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using AppBoot.DependencyInjection;
using Contracts.Sales;
using DataAccess;
using Sales.DataModel.SalesLT;

namespace Sales.Services;

[Service(typeof(ICustomerService))]
class CustomerService(IRepository repository) : ICustomerService
{
    public CustomerData[] GetCustomersWithOrders()
    {
        return GetCustomersWithOrdersFilteredBy(x => true);
    }

    private CustomerData[] GetCustomersWithOrdersFilteredBy(Expression<Func<Customer, bool>> filter)
    {
        var q = repository.GetEntities<Customer>()
            .Where(c => c.SalesOrderHeaders != null && c.SalesOrderHeaders.Any())
            .Where(filter)
            .OrderBy(c => c.CompanyName)
            .Select(c => new CustomerData
            {
                Id = c.CustomerID,
                CompanyName = c.CompanyName,
                SalesPerson = c.SalesPerson
            });

        return q.ToArray();
    }

    public CustomerData[] GetCustomersWithOrdersStartingWith(string prefix)
    {
        Expression<Func<Customer, bool>> filter;
        if (string.IsNullOrEmpty(prefix))
            filter = x => true;
        else
            filter = c => c.CompanyName != null && c.CompanyName.StartsWith(prefix);

        return GetCustomersWithOrdersFilteredBy(filter);
    }

    public CustomerData[] GetCustomersWithOrdersContaining(string fragment)
    {
        Expression<Func<Customer, bool>> filter;
        if (string.IsNullOrEmpty(fragment))
            filter = x => true;
        else
            filter = c => c.CompanyName != null && c.CompanyName.Contains(fragment);

        return GetCustomersWithOrdersFilteredBy(filter);
    }
}
