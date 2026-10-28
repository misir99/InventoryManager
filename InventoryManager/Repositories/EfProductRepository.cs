using InventoryManager.Data;
using InventoryManager.Exceptions;
using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public EfProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();

        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            var existing = await _context.Products.FindAsync(product.Id);
            if (existing == null)
                throw new ProductNotFoundException(product.Id);

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Category = product.Category;
            existing.Quantity = product.Quantity;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
                throw new ProductNotFoundException(id);

            _context.Products.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }

}

