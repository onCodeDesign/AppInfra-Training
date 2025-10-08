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
    public partial class Product
    {
        // Key Properties
        public int ProductID { get; set; }

        // Scalar Properties
        public string Name { get; set; } = "";
        public string ProductNumber { get; set; } = "";
        public string? Color { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string? Size { get; set; }
        public decimal? Weight { get; set; }
        public int? ProductCategoryID { get; set; }
        public int? ProductModelID { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
        public byte[]? ThumbNailPhoto { get; set; }
        public string? ThumbnailPhotoFileName { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public ProductCategory? ProductCategory { get; set; } //Column: ProductCategoryID, FK: FK_Product_ProductCategory_ProductCategoryID
        public ProductModel? ProductModel { get; set; } //Column: ProductModelID, FK: FK_Product_ProductModel_ProductModelID

        // Reverse navigation properties
        public List<SalesOrderDetail> SalesOrderDetails { get; set; }

        public Product()
        {
            this.SalesOrderDetails = new List<SalesOrderDetail>();
        }
    }
}