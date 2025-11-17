using Microsoft.EntityFrameworkCore;
using ProductsManagement.DataModel.SalesLT;

namespace ProductsManagement.Infrastructure;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductModel> ProductModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductBuilder());
        modelBuilder.ApplyConfiguration(new ProductCategoryBuilder());
        modelBuilder.ApplyConfiguration(new ProductModelBuilder());
    }
}
