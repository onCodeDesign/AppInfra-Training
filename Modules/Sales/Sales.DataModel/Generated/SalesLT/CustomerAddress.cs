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
    public partial class CustomerAddress
    {
        // Key Properties
        public int CustomerID { get; set; }
        public int AddressID { get; set; }

        // Scalar Properties
        public string AddressType { get; set; } = "";
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public Address? Address { get; set; } //Column: AddressID, FK: FK_CustomerAddress_Address_AddressID
        public Customer? Customer { get; set; } //Column: CustomerID, FK: FK_CustomerAddress_Customer_CustomerID

    }
}