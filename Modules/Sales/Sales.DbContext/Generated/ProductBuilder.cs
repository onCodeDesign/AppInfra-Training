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
    public partial class ProductBuilder : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.ToTable("Product", "SalesLT");
            entity.HasKey(e => e.ProductID);

            entity.Property(e => e.ProductID)
                .HasColumnName("ProductID")
                .HasColumnType("int")
                .UseIdentityColumn();

            entity.Property(e => e.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ProductNumber)
                .HasColumnName("ProductNumber")
                .HasColumnType("nvarchar(25)")
                .IsRequired()
                .HasMaxLength(25);

            entity.Property(e => e.Color)
                .HasColumnName("Color")
                .HasColumnType("nvarchar(15)")
                .HasMaxLength(15);

            entity.Property(e => e.StandardCost)
                .HasColumnName("StandardCost")
                .HasColumnType("money");

            entity.Property(e => e.ListPrice)
                .HasColumnName("ListPrice")
                .HasColumnType("money");

            entity.Property(e => e.Size)
                .HasColumnName("Size")
                .HasColumnType("nvarchar(5)")
                .HasMaxLength(5);

            entity.Property(e => e.Weight)
                .HasColumnName("Weight")
                .HasColumnType("decimal(8, 2)");

            entity.Property(e => e.SellStartDate)
                .HasColumnName("SellStartDate")
                .HasColumnType("datetime");

            entity.Property(e => e.SellEndDate)
                .HasColumnName("SellEndDate")
                .HasColumnType("datetime");

            entity.Property(e => e.DiscontinuedDate)
                .HasColumnName("DiscontinuedDate")
                .HasColumnType("datetime");

            entity.Property(e => e.ThumbNailPhoto)
                .HasColumnName("ThumbNailPhoto")
                .HasColumnType("varbinary(MAX)");

            entity.Property(e => e.ThumbnailPhotoFileName)
                .HasColumnName("ThumbnailPhotoFileName")
                .HasColumnType("nvarchar(50)")
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

            entity.HasOne(e => e.ProductCategory)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.ProductCategoryID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Product_ProductCategory_ProductCategoryID");

            entity.HasOne(e => e.ProductModel)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.ProductModelID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Product_ProductModel_ProductModelID");

        }
    }
}