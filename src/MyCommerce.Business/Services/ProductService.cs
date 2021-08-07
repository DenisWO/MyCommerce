using MyCommerce.Business.Interfaces;
using MyCommerce.Business.Models;
using MyCommerce.Business.Models.Validations;
using MyCommerce.Business.Notifications;
using System;
using System.Threading.Tasks;

namespace MyCommerce.Business.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository, INotificator notificator) : base(notificator)
        {
            _productRepository = productRepository;
        }
        public async Task Add(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product))
                return;

            await _productRepository.Add(product);
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }

        public async Task Remove(Guid id)
        {
            await _productRepository.Remove(id);
        }

        public async Task Update(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product))
                return;

            await _productRepository.Update(product);
        }
    }
}
