using System.CodeDom.Compiler;

namespace ProductsManagement.DataModel.SalesLT;

[GeneratedCode("Manual", "1.0.0.0")]
[Serializable]
public partial class ProductCategory
{
    public int ProductCategoryID { get; set; }
    public int? ParentProductCategoryID { get; set; }
    public string Name { get; set; } = "";
    public Guid Rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }

    public ProductCategory? ParentProductCategory { get; set; }
    public List<Product> Products { get; set; }
    public List<ProductCategory> ChildCategories { get; set; }

    public ProductCategory()
    {
        this.Products = new List<Product>();
        this.ChildCategories = new List<ProductCategory>();
    }
}
