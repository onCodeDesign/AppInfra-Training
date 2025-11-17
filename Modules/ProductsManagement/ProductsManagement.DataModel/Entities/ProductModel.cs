using System.CodeDom.Compiler;

namespace ProductsManagement.DataModel.SalesLT;

[GeneratedCode("Manual", "1.0.0.0")]
[Serializable]
public partial class ProductModel
{
    public int ProductModelID { get; set; }
    public string Name { get; set; } = "";
    public string? CatalogDescription { get; set; }
    public Guid Rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }

    public List<Product> Products { get; set; }

    public ProductModel()
    {
        this.Products = new List<Product>();
    }
}
