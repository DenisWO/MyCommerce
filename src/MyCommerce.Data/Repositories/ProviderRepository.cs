using Microsoft.EntityFrameworkCore;
using MyCommerce.Business.Interfaces;
using MyCommerce.Business.Models;
using MyCommerce.Data.Context;
using System;
using System.Threading.Tasks;

namespace MyCommerce.Data.Repositories
{
    public class ProviderRepository : Repository<Provider>, IProviderRepository
    {
        public ProviderRepository(MyCommerceDbContext db) : base(db) { }
        public async Task<Provider> GetProviderWithAddress(Guid id)
        {
            return await dbContext.Providers.AsNoTracking().Include(add => add.Address).FirstOrDefaultAsync(prov => prov.Id == id);
        }

        public async Task<Provider> GetProviderWithProductsAndAddress(Guid id)
        {
            return await dbContext.Providers.AsNoTracking()
                .Include(add => add.Address)
                .Include(prod => prod.Products)
                .FirstOrDefaultAsync(prov => prov.Id == id);
        }
    }
}
