using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManager.Models;

namespace InventoryManager.Repositories
{
    public interface IProductRepository
    {
        // 1. dohvati sve
        List<Product> getAll();
        // 2. dohvati po Id-u
        Product? GetById(int id);
        // 3. dodaj
        void Add(Product product);
        // 4. izmijeni
        void Update(Product product);
        // 5. obriši
        void Delete(int id);
    }
}
