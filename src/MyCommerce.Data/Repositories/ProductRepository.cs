using Microsoft.EntityFrameworkCore;
using MyCommerce.Business.Interfaces;
using MyCommerce.Business.Models;
using MyCommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommerce.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MyCommerceDbContext db) : base(db) {}
        public async Task<IEnumerable<Product>> GetProductsByProvider(Guid providerId)
        {
            return await Get(prod => prod.ProviderId == providerId);
        }

        public async Task<IEnumerable<Product>> GetProductsWithProviders()
        {
            return await dbContext.Products.AsNoTracking().Include(prov => prov.Provider).OrderBy(prod => prod.Name).ToListAsync();
        }

        public async Task<Product> GetProductWithProvider(Guid id)
        {
            return await dbContext.Products.AsNoTracking().Include(prov => prov.Provider).FirstOrDefaultAsync(prod => prod.Id == id);            
        }
    }
}
