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
    public partial class CustomerAddressBuilder : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> entity)
        {
            entity.ToTable("CustomerAddress", "SalesLT");
            entity.HasKey(e => new { e.CustomerID, e.AddressID });

            entity.Property(e => e.AddressType)
                .HasColumnName("AddressType")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Rowguid)
                .HasColumnName("rowguid")
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("newid()")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            entity.HasOne(e => e.Address)
                .WithMany(p => p.CustomerAddresses)
                .HasForeignKey(p => p.AddressID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CustomerAddress_Address_AddressID");

            entity.HasOne(e => e.Customer)
                .WithMany(p => p.CustomerAddresses)
                .HasForeignKey(p => p.CustomerID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CustomerAddress_Customer_CustomerID");

        }
    }
}