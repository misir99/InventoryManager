using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManager.Data;
using InventoryManager.Exceptions;
using InventoryManager.Models;

namespace InventoryManager.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public EfProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();

        }

        public Product? GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            var existing = _context.Products.Find(product.Id);
            if (existing == null)
                throw new ProductNotFoundException(product.Id);

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Category = product.Category;
            existing.Quantity = product.Quantity;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.Products.Find(id);
            if (existing == null)
                throw new ProductNotFoundException(id);

            _context.Products.Remove(existing);
            _context.SaveChanges();
        }
                
            }
        }
    