using MyCommerce.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCommerce.Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByProvider(Guid providerId);
        Task<IEnumerable<Product>> GetProductsWithProviders();
        Task<Product> GetProductWithProvider(Guid id);

    }
}
