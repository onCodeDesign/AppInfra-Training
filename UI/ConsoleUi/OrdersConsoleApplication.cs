using AppBoot.DependencyInjection;
using Contracts.Sales;

namespace ConsoleUi;

[Service(typeof(OrdersConsoleApplication))]
class OrdersConsoleApplication(IConsole console, IOrderingService orderingService)
{
    public void ShowAllOrders()
    {
        console.WriteLine("OrdersConsole: Show all orders function");
        string customerName = console.AskInput("Enter customer last name: ");

        SalesOrderInfo[] orders = orderingService.GetOrdersInfo(customerName);

        console.WriteLine($"Orders for customer {customerName}: "); //Test data: Abel | Smith | Adams
        foreach (SalesOrderInfo salesOrderInfo in orders)
        {
            console.WriteEntity(salesOrderInfo);
        }
    }
}