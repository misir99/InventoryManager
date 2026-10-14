using InventoryManager.Models;
using Microsoft.Data.SqlClient;

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
            throw new NotImplementedException();
        }

        public Product? GetById(int id)
        {
            throw new NotImplementedException();
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
                          SET NAme = @Name, 
Price = @Price,
Category = @Category, 
Quantity = @Quantity, 
WHERE Id = @Id";                      

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Category", product.Category);
            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"DELETE FROM Products WHERE Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}