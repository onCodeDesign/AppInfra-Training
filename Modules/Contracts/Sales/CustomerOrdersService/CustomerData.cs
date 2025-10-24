using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Sales.CustomerOrdersService;

public class CustomerData
{
   public int Id { get; set; }
   public string? SalesPerson { get; set; }
   public string? CompanyName { get; set; }
}