// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Global
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System.CodeDom.Compiler;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.DataModel.SalesLT;

namespace Sales.DbContext
{
    public partial class SalesOrderHeaderBuilder : IEntityTypeConfiguration<SalesOrderHeader>
    {
        public void Configure(EntityTypeBuilder<SalesOrderHeader> entity)
        {
            entity.ToTable("SalesOrderHeader", "SalesLT", tb => tb.HasTrigger("uSalesOrderHeader"));
            entity.HasKey(e => e.SalesOrderID);

            entity.Property(e => e.SalesOrderID)
                .HasColumnName("SalesOrderID")
                .HasColumnType("int")
                .UseIdentityColumn();

            entity.Property(e => e.RevisionNumber)
                .HasColumnName("RevisionNumber")
                .HasColumnType("tinyint")
                .HasDefaultValueSql("0");

            entity.Property(e => e.OrderDate)
                .HasColumnName("OrderDate")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            entity.Property(e => e.DueDate)
                .HasColumnName("DueDate")
                .HasColumnType("datetime");

            entity.Property(e => e.ShipDate)
                .HasColumnName("ShipDate")
                .HasColumnType("datetime");

            entity.Property(e => e.Status)
                .HasColumnName("Status")
                .HasColumnType("tinyint")
                .HasDefaultValueSql("1");

            entity.Property(e => e.OnlineOrderFlag)
                .HasColumnName("OnlineOrderFlag")
                .HasColumnType("bit");

            entity.Property(e => e.SalesOrderNumber)
                .HasColumnName("SalesOrderNumber")
                .HasColumnType("nvarchar(25)")
                .IsRequired()
                .HasMaxLength(25)
                .HasComputedColumnSql("(isnull(N'SO'+CONVERT([nvarchar](23),[SalesOrderID]),N'*** ERROR ***'))");

            entity.Property(e => e.PurchaseOrderNumber)
                .HasColumnName("PurchaseOrderNumber")
                .HasColumnType("nvarchar(25)")
                .HasMaxLength(25);

            entity.Property(e => e.AccountNumber)
                .HasColumnName("AccountNumber")
                .HasColumnType("nvarchar(15)")
                .HasMaxLength(15);

            entity.Property(e => e.ShipMethod)
                .HasColumnName("ShipMethod")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CreditCardApprovalCode)
                .HasColumnName("CreditCardApprovalCode")
                .HasColumnType("varchar(15)")
                .HasMaxLength(15);

            entity.Property(e => e.SubTotal)
                .HasColumnName("SubTotal")
                .HasColumnType("money")
                .HasDefaultValueSql("0.00");

            entity.Property(e => e.TaxAmt)
                .HasColumnName("TaxAmt")
                .HasColumnType("money")
                .HasDefaultValueSql("0.00");

            entity.Property(e => e.Freight)
                .HasColumnName("Freight")
                .HasColumnType("money")
                .HasDefaultValueSql("0.00");

            entity.Property(e => e.TotalDue)
                .HasColumnName("TotalDue")
                .HasColumnType("money")
                .HasComputedColumnSql("(isnull(([SubTotal]+[TaxAmt])+[Freight],(0)))");

            entity.Property(e => e.Comment)
                .HasColumnName("Comment")
                .HasColumnType("nvarchar(MAX)");

            entity.Property(e => e.Rowguid)
                .HasColumnName("rowguid")
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("newid()")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            entity.HasOne(e => e.BillToAddress)
                .WithMany(p => p.SalesOrderHeadersBillToAddress)
                .HasForeignKey(p => p.BillToAddressID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SalesOrderHeader_Address_BillTo_AddressID");

            entity.HasOne(e => e.ShipToAddress)
                .WithMany(p => p.SalesOrderHeadersShipToAddress)
                .HasForeignKey(p => p.ShipToAddressID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SalesOrderHeader_Address_ShipTo_AddressID");

            entity.HasOne(e => e.Customer)
                .WithMany(p => p.SalesOrderHeaders)
                .HasForeignKey(p => p.CustomerID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SalesOrderHeader_Customer_CustomerID");

        }
    }
}