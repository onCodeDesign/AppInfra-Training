using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsManagement.DataModel.SalesLT;

namespace ProductsManagement.Infrastructure;

public class ProductBuilder : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        entity.ToTable("Product", "SalesLT");
        entity.HasKey(e => e.ProductID);

        entity.Property(e => e.ProductID)
            .HasColumnName("ProductID")
            .UseIdentityColumn();

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(e => e.ProductNumber)
            .IsRequired()
            .HasMaxLength(25);

        entity.Property(e => e.Color)
            .HasMaxLength(15);

        entity.Property(e => e.StandardCost)
            .HasColumnType("money");

        entity.Property(e => e.ListPrice)
            .HasColumnType("money");

        entity.Property(e => e.Size)
            .HasMaxLength(5);

        entity.Property(e => e.Rowguid)
            .HasColumnName("rowguid")
            .HasDefaultValueSql("newid()")
            .ValueGeneratedOnAdd();

        entity.Property(e => e.ModifiedDate)
            .HasDefaultValueSql("getdate()");

        entity.HasOne(e => e.ProductCategory)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.ProductCategoryID)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.ProductModel)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.ProductModelID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
