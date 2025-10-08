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
    public partial class ProductModelBuilder : IEntityTypeConfiguration<ProductModel>
    {
        public void Configure(EntityTypeBuilder<ProductModel> entity)
        {
            entity.ToTable("ProductModel", "SalesLT");
            entity.HasKey(e => e.ProductModelID);

            entity.Property(e => e.ProductModelID)
                .HasColumnName("ProductModelID")
                .HasColumnType("int")
                .UseIdentityColumn();

            entity.Property(e => e.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CatalogDescription)
                .HasColumnName("CatalogDescription")
                .HasColumnType("xml");

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