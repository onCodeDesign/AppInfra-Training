namespace Contracts.Sales;

public class CustomerImportResult
{
    public int CustomersAdded { get; set; }
    public int CustomersUpdated { get; set; }
    public int CustomersSkipped { get; set; }
    public int TotalPersonsProcessed { get; set; }
}
