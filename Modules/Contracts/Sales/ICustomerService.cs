namespace Contracts.Sales;

public interface ICustomerService
{
    CustomerData[] GetCustomersWithOrders();

    CustomerData[] GetCustomersWithOrdersStartingWith(string prefix);

    CustomerData[] GetCustomersWithOrdersContaining(string fragment);
}
