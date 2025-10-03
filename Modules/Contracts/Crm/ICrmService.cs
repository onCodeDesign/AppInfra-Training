namespace Contracts.Crm;

public interface ICrmService
{
    CustomerInfo GetCustomerInfo(string customerName);
}

public class CustomerInfo
{
}