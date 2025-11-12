namespace Contracts.Sales;

public interface ICustomerService
{
    CustomerData[] GetCustomersWithOrders();
}
