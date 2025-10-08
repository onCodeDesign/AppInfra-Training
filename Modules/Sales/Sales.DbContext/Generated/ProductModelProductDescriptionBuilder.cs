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
    public partial class ProductModelProductDescriptionBuilder : IEntityTypeConfiguration<ProductModelProductDescription>
    {
        public void Configure(EntityTypeBuilder<ProductModelProductDescription> entity)
        {
            entity.ToTable("ProductModelProductDescription", "SalesLT");
            entity.HasKey(e => new { e.ProductModelID, e.ProductDescriptionID, e.Culture });

            entity.Property(e => e.Culture)
                .HasColumnName("Culture")
                .HasColumnType("nchar(6)")
                .IsRequired()
                .HasMaxLength(6);

            entity.Property(e => e.Rowguid)
                .HasColumnName("rowguid")
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("newid()")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            entity.HasOne(e => e.ProductDescription)
                .WithMany(p => p.ProductModelProductDescriptions)
                .HasForeignKey(p => p.ProductDescriptionID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProductModelProductDescription_ProductDescription_ProductDescriptionID");

            entity.HasOne(e => e.ProductModel)
                .WithMany(p => p.ProductModelProductDescriptions)
                .HasForeignKey(p => p.ProductModelID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProductModelProductDescription_ProductModel_ProductModelID");

        }
    }
}