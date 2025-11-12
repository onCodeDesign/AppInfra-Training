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
        var query = repository.GetEntities<Customer>()
            .Where(c => c.SalesOrderHeaders != null && c.SalesOrderHeaders.Any())
            .OrderBy(c => c.CompanyName)
            .Select(c => new CustomerData
            {
                Id = c.CustomerID,
                CompanyName = c.CompanyName,
                SalesPerson = c.SalesPerson
            });

        return query.ToArray();
    }
}
