using MyCommerce.Business.Models;
using System;
using System.Threading.Tasks;

namespace MyCommerce.Business.Interfaces
{
    public interface IProviderRepository : IRepository<Provider>
    {
        Task<Provider> GetProviderWithAddress(Guid id);
        Task<Provider> GetProviderWithProductsAndAddress(Guid id);
    }
}
