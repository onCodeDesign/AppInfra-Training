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
    public partial class CustomerBuilder : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.ToTable("Customer", "SalesLT");
            entity.HasKey(e => e.CustomerID);

            entity.Property(e => e.CustomerID)
                .HasColumnName("CustomerID")
                .HasColumnType("int")
                .UseIdentityColumn();

            entity.Property(e => e.NameStyle)
                .HasColumnName("NameStyle")
                .HasColumnType("bit");

            entity.Property(e => e.Title)
                .HasColumnName("Title")
                .HasColumnType("nvarchar(8)")
                .HasMaxLength(8);

            entity.Property(e => e.FirstName)
                .HasColumnName("FirstName")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.MiddleName)
                .HasColumnName("MiddleName")
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50);

            entity.Property(e => e.LastName)
                .HasColumnName("LastName")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Suffix)
                .HasColumnName("Suffix")
                .HasColumnType("nvarchar(10)")
                .HasMaxLength(10);

            entity.Property(e => e.CompanyName)
                .HasColumnName("CompanyName")
                .HasColumnType("nvarchar(128)")
                .HasMaxLength(128);

            entity.Property(e => e.SalesPerson)
                .HasColumnName("SalesPerson")
                .HasColumnType("nvarchar(256)")
                .HasMaxLength(256);

            entity.Property(e => e.EmailAddress)
                .HasColumnName("EmailAddress")
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50);

            entity.Property(e => e.Phone)
                .HasColumnName("Phone")
                .HasColumnType("nvarchar(25)")
                .HasMaxLength(25);

            entity.Property(e => e.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasColumnType("varchar(128)")
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.PasswordSalt)
                .HasColumnName("PasswordSalt")
                .HasColumnType("varchar(10)")
                .IsRequired()
                .HasMaxLength(10);

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