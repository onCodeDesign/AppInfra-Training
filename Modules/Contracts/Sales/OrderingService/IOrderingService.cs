namespace Contracts.Sales.OrderingService;

public interface IOrderingService
{
    SalesOrderResult PlaceOrder(string customerName, OrderRequest request);

    SalesOrderInfo[] GetOrdersInfo(string customerName);
}