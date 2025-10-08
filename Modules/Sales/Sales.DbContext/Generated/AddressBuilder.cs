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
    public partial class AddressBuilder : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entity)
        {
            entity.ToTable("Address", "SalesLT");
            entity.HasKey(e => e.AddressID);

            entity.Property(e => e.AddressID)
                .HasColumnName("AddressID")
                .HasColumnType("int")
                .UseIdentityColumn();

            entity.Property(e => e.AddressLine1)
                .HasColumnName("AddressLine1")
                .HasColumnType("nvarchar(60)")
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(e => e.AddressLine2)
                .HasColumnName("AddressLine2")
                .HasColumnType("nvarchar(60)")
                .HasMaxLength(60);

            entity.Property(e => e.City)
                .HasColumnName("City")
                .HasColumnType("nvarchar(30)")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.StateProvince)
                .HasColumnName("StateProvince")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CountryRegion)
                .HasColumnName("CountryRegion")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.PostalCode)
                .HasColumnName("PostalCode")
                .HasColumnType("nvarchar(15)")
                .IsRequired()
                .HasMaxLength(15);

            entity.Property(e => e.Rowguid)
                .HasColumnName("rowguid")
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("newid()")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

        }
    }
}