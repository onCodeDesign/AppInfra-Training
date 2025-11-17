using AppBoot.DependencyInjection;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ProductsManagement.Infrastructure;

[Service(typeof(IDbContextFactory))]
public class DbContextFactory : IDbContextFactory
{
    // THIS IS Demo code. DO NOT STORE CONNECTION STRINGS IN SOURCE CODE in PRODUCTION CODE!
    private static readonly string connectionString = 
        "Server=.\\SQLEXPRESS;Database=AdventureWorksLT2019;Trusted_Connection=True;TrustServerCertificate=True;";

    public IDbContextWrapper CreateContext()
    {
        DbContextOptions<ProductsDbContext> connectionOptions = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        var context = new ProductsDbContext(connectionOptions);
        return new DbContextWrapper(context);
    }
}
