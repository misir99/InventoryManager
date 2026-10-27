using InventoryManager.Models;
using Microsoft.Data.SqlClient;
using InventoryManager.Exceptions;

namespace InventoryManager.Repositories
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public SqlProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Product> GetAll()
        {
            var products = new List<Product>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT Id, Name, Price, Category, Quantity FROM Products";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var product = new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Category = reader.GetString(3),
                    Quantity = reader.GetInt32(4)
                };
                products.Add(product);
            }

            return products;
        }

        public Product? GetById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT Id, Name, Price, Category, Quantity FROM Products WHERE Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Category = reader.GetString(3),
                    Quantity = reader.GetInt32(4)
                };
            }

            return null;
        }

        public void Add(Product product)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"INSERT INTO Products (Name, Price, Category, Quantity)
                           VALUES (@Name, @Price, @Category, @Quantity)";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Category", product.Category);
            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

            cmd.ExecuteNonQuery();
        }

        public void Update(Product product)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"UPDATE Products
                           SET Name = @Name,
                               Price = @Price,
                               Category = @Category,
                               Quantity = @Quantity
                           WHERE Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", product.Id);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Category", product.Category);
            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

            int affected = cmd.ExecuteNonQuery();
            if (affected == 0)
                throw new ProductNotFoundException(product.Id);
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "DELETE FROM Products WHERE Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            int affected = cmd.ExecuteNonQuery();
            if (affected == 0)
                throw new ProductNotFoundException(id);
        }
    }
}