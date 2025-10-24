namespace Contracts.Sales.CustomerOrdersService;

public interface ICustomerOrdersService
{
    IEnumerable<CustomerData> GetCustomersWithOrders();
}