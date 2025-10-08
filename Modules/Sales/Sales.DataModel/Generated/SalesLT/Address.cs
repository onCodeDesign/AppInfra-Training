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
    public partial class Address
    {
        // Key Properties
        public int AddressID { get; set; }

        // Scalar Properties
        public string AddressLine1 { get; set; } = "";
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = "";
        public string StateProvince { get; set; } = "";
        public string CountryRegion { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Reverse navigation properties
        public List<CustomerAddress> CustomerAddresses { get; set; }
        public List<SalesOrderHeader> SalesOrderHeadersBillToAddress { get; set; }
        public List<SalesOrderHeader> SalesOrderHeadersShipToAddress { get; set; }

        public Address()
        {
            this.CustomerAddresses = new List<CustomerAddress>();
            this.SalesOrderHeadersBillToAddress = new List<SalesOrderHeader>();
            this.SalesOrderHeadersShipToAddress = new List<SalesOrderHeader>();
        }
    }
}