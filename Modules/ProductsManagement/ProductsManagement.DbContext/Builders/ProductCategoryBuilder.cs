using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsManagement.DataModel.SalesLT;

namespace ProductsManagement.Infrastructure;

public class ProductCategoryBuilder : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> entity)
    {
        entity.ToTable("ProductCategory", "SalesLT");
        entity.HasKey(e => e.ProductCategoryID);

        entity.Property(e => e.ProductCategoryID)
            .HasColumnName("ProductCategoryID")
            .UseIdentityColumn();

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(e => e.Rowguid)
            .HasColumnName("rowguid")
            .HasDefaultValueSql("newid()")
            .ValueGeneratedOnAdd();

        entity.Property(e => e.ModifiedDate)
            .HasDefaultValueSql("getdate()");

        entity.HasOne(e => e.ParentProductCategory)
            .WithMany(p => p.ChildCategories)
            .HasForeignKey(p => p.ParentProductCategoryID)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_ProductCategory_ProductCategory_ParentProductCategoryID_ProductCategoryID");
    }
}
