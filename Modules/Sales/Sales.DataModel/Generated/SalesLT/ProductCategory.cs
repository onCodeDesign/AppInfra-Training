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
    public partial class ProductCategory
    {
        // Key Properties
        public int ProductCategoryID { get; set; }

        // Scalar Properties
        public int? ParentProductCategoryID { get; set; }
        public string Name { get; set; } = "";
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public ProductCategory? ParentProductCategory { get; set; } //Column: ParentProductCategoryID, FK: FK_ProductCategory_ProductCategory_ParentProductCategoryID_ProductCategoryID

        // Reverse navigation properties
        public List<Product> Products { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

        public ProductCategory()
        {
            this.Products = new List<Product>();
            this.ProductCategories = new List<ProductCategory>();
        }
    }
}