using Microsoft.EntityFrameworkCore;
using MyCommerce.Business.Interfaces;
using MyCommerce.Business.Models;
using MyCommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCommerce.Data.Repositories
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(MyCommerceDbContext db) : base(db) {}

        public async Task<Address> GetAddressByProvider(Guid providerId)
        {
            return await dbContext.Addresses.AsNoTracking().FirstOrDefaultAsync(prov => prov.ProviderId == providerId);
        }
    }
}
