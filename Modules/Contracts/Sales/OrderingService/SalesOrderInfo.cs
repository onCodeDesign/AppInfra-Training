using System;

namespace Contracts.Sales.OrderingService;

public class SalesOrderInfo
{
	public string CustomerName { get; set; }
	public string Number { get; set; }

	public string ShipToCity { get; set; }

	public DateTime DueDate { get; set; }

	public decimal TotalDue { get; set; }
}