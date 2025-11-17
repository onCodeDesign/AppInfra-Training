using AppBoot.DependencyInjection;
using Contracts.ProductsManagement;
using DataAccess;
using ProductsManagement.DataModel.SalesLT;

namespace ProductsManagement.Services;

[Service(typeof(IProductCategoryService))]
internal class ProductCategoryService(IRepository repository) : IProductCategoryService
{
    public int AddProductCategory(ProductCategoryData categoryData)
    {
        using (IUnitOfWork uof = repository.CreateUnitOfWork())
        {
            var category = new ProductCategory
            {
                Name = categoryData.Name,
                ParentProductCategoryID = categoryData.ParentProductCategoryID
            };

            uof.Add(category);
            uof.SaveChanges();

            return category.ProductCategoryID;
        }
    }
}
