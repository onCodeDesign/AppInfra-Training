// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Global
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System.CodeDom.Compiler;

namespace Sales.DataModel.SalesLT
{
    [GeneratedCode("Geco", "1.5.0.0")]
    [Serializable]
    public partial class ProductModel
    {
        // Key Properties
        public int ProductModelID { get; set; }

        // Scalar Properties
        public string Name { get; set; } = "";
        public string? CatalogDescription { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Reverse navigation properties
        public List<Product> Products { get; set; }
        public List<ProductModelProductDescription> ProductModelProductDescriptions { get; set; }

        public ProductModel()
        {
            this.Products = new List<Product>();
            this.ProductModelProductDescriptions = new List<ProductModelProductDescription>();
        }
    }
}