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
    public partial class SalesOrderDetail
    {
        // Key Properties
        public int SalesOrderID { get; set; }
        public int SalesOrderDetailID { get; set; }

        // Scalar Properties
        public short OrderQty { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceDiscount { get; set; }
        public decimal LineTotal { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public Product? Product { get; set; } //Column: ProductID, FK: FK_SalesOrderDetail_Product_ProductID
        public SalesOrderHeader? SalesOrderHeader { get; set; } //Column: SalesOrderID, FK: FK_SalesOrderDetail_SalesOrderHeader_SalesOrderID

    }
}