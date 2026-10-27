using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Exceptions
{
   public class ProductNotFoundException : Exception
    {
        public int ProductId { get;  }
        public ProductNotFoundException(int productId)
            : base($"Proizvod s Id-om {productId} ne postoji.")
        {
            ProductId = productId;
        }
    }

}
