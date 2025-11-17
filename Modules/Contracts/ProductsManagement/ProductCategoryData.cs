namespace Contracts.ProductsManagement;

public class ProductCategoryData
{
    public int ProductCategoryID { get; set; }
    public int? ParentProductCategoryID { get; set; }
    public string Name { get; set; } = "";
    public DateTime ModifiedDate { get; set; }
}
