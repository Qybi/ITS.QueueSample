using ITS.QueueSample.Models;
using Npgsql;
using Dapper;
using Microsoft.Extensions.Configuration;
using ITS.QueueSample.IDataAccess;

namespace ITS.QueueSample.DataAccess;

public class ProductsDataService : IProductsDataService
{
    private readonly string _connectionString;

    public ProductsDataService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("db") ?? throw new Exception("Missing connection string 'db'.");
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string query = """
            SELECT 
                id,
                name,
                code,
                price
            FROM products;
            """;
        return await connection.QueryAsync<Product>(query);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string query = """
            SELECT 
                id,
                name,
                code,
                price
            FROM products
            WHERE id = @id;
            """;
        return await connection.QueryFirstOrDefaultAsync<Product>(query, new { id });
    }

    public async Task InsertAsync(Product product)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string query = """
            INSERT INTO products (name, code, price)
            VALUES (@Name, @Code, @Price)  
            """;
        await connection.ExecuteAsync(query, product);
    }

    public async Task UpdateAsync(Product product)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string query = """
            UPDATE products
            SET 
                name = @Name,
                code = @Code,
                price = @Price
            WHERE id = @Id
            """;
        await connection.ExecuteAsync(query, product);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string query = """
            DELETE FROM products WHERE id = @id;
            """;
        await connection.ExecuteAsync(query, new { id });
    }
}
