using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsManagement.DataModel.SalesLT;

namespace ProductsManagement.Infrastructure;

public class ProductModelBuilder : IEntityTypeConfiguration<ProductModel>
{
    public void Configure(EntityTypeBuilder<ProductModel> entity)
    {
        entity.ToTable("ProductModel", "SalesLT");
        entity.HasKey(e => e.ProductModelID);

        entity.Property(e => e.ProductModelID)
            .HasColumnName("ProductModelID")
            .UseIdentityColumn();

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(e => e.CatalogDescription)
            .HasColumnType("xml");

        entity.Property(e => e.Rowguid)
            .HasColumnName("rowguid")
            .HasDefaultValueSql("newid()")
            .ValueGeneratedOnAdd();

        entity.Property(e => e.ModifiedDate)
            .HasDefaultValueSql("getdate()");
    }
}
