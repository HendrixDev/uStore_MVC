using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uStore.DATA.EF
{
   public class LinqDAL
    {
        uStoreEntities db = new uStoreEntities();

        public List<Product> OrderByLowestPrice()
        {
            var products = db.Products.OrderBy(p => p.Price).ToList();
            return products;
        }
        public List<Product> OrderByHighestPrice()
        {
            var products = db.Products.OrderByDescending(p => p.Price).ToList();
            return products;
        }
    }
}
