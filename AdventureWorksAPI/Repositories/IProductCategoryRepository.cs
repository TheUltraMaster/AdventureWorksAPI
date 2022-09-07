using AdventureWorksNS.Data;

namespace AdventureWorksAPI.Repositories
{
    public interface IProductCategoryRepository
    {

        Task<ProductCategory> CreateAsync(ProductCategory c);
        Task<IEnumerable<ProductCategory>> RetrieveAllAsync();
        Task<ProductCategory?> RetrieveAsync(int id);
        Task<ProductCategory?> UpdateAsync(int id, ProductCategory c);
        Task<bool?> DeleteAsync(int id);
    }
}
