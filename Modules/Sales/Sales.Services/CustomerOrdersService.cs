using AppBoot.DependencyInjection;
using Contracts.Sales.CustomerOrdersService;
using DataAccess;
using Sales.DataModel.SalesLT;

namespace Sales.Services;

[Service(typeof(ICustomerOrdersService))]
class CustomerOrdersService(IRepository rep) : ICustomerOrdersService
{
    public IEnumerable<CustomerData> GetCustomersWithOrders()
    {
        var query = rep.GetEntities<Customer>()
            .Where(c => c.SalesOrderHeaders.Any())
            .OrderBy(c => c.CompanyName)
            .Select(c => new CustomerData
            {
                Id = c.CustomerID,
                CompanyName = c.CompanyName,
                SalesPerson = c.SalesPerson
            });

        return query.AsEnumerable();
    }
}