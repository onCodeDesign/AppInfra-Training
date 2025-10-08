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
    public partial class ProductModelProductDescription
    {
        // Key Properties
        public int ProductModelID { get; set; }
        public int ProductDescriptionID { get; set; }
        public string Culture { get; set; }

        // Scalar Properties
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public ProductDescription? ProductDescription { get; set; } //Column: ProductDescriptionID, FK: FK_ProductModelProductDescription_ProductDescription_ProductDescriptionID
        public ProductModel? ProductModel { get; set; } //Column: ProductModelID, FK: FK_ProductModelProductDescription_ProductModel_ProductModelID

    }
}