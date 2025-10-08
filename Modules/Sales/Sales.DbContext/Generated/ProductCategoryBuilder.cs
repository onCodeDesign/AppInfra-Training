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
    public partial class ProductCategoryBuilder : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> entity)
        {
            entity.ToTable("ProductCategory", "SalesLT");
            entity.HasKey(e => e.ProductCategoryID);

            entity.Property(e => e.ProductCategoryID)
                .HasColumnName("ProductCategoryID")
                .HasColumnType("int")
                .UseIdentityColumn();

            entity.Property(e => e.Name)
                .HasColumnName("Name")
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

            entity.HasOne(e => e.ParentProductCategory)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(p => p.ParentProductCategoryID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProductCategory_ProductCategory_ParentProductCategoryID_ProductCategoryID");

        }
    }
}