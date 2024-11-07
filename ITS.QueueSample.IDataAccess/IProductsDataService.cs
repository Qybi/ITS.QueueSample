using ITS.QueueSample.Models;

namespace ITS.QueueSample.IDataAccess;

public interface IProductsDataService
{
	Task DeleteAsync(int id);
	Task<Product?> GetProductByIdAsync(int Id);
	Task<IEnumerable<Product>> GetProductsAsync();
	Task InsertAsync(Product product);
	Task UpdateAsync(Product product);
}