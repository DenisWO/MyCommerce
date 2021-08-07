using MyCommerce.Business.Interfaces;
using MyCommerce.Business.Models;
using MyCommerce.Business.Models.Validations;
using MyCommerce.Business.Notifications;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommerce.Business.Services
{
    public class ProviderService : BaseService, IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IAddressRepository _addressRepository;

        public ProviderService(IProviderRepository providerRepository, IAddressRepository addressRepository, INotificator notificator) : base(notificator)
        {
            _providerRepository = providerRepository;
            _addressRepository = addressRepository;
        }
        public async Task Add(Provider provider)
        {
            if (!ExecuteValidation(new ProviderValidation(), provider) && !ExecuteValidation(new AddressValidation(), provider.Address))
                return;

            if (_providerRepository.Get(p => p.Document == provider.Document).Result.Any())
            {
                Notify("Já existe um fornecedor com este documento informado!");
                return;
            }

            await _providerRepository.Add(provider);
        }

        public void Dispose()
        {
            _providerRepository?.Dispose();
            _addressRepository?.Dispose();
        }

        public async Task Remove(Guid id)
        {
            if (_providerRepository.GetProviderWithAddress(id).Result.Products.Any())
            {
                Notify("O fornecedor possui produtos cadastrados!");
                return;
            }

            await _providerRepository.Remove(id);
        }

        public async Task Update(Provider provider)
        {
            if (!ExecuteValidation(new ProviderValidation(), provider))
                return;

            if (_providerRepository.Get(p => p.Document == provider.Document && p.Id != provider.Id).Result.Any())
            {
                Notify("Já existe um fornecedor com este documento informado!");
                return;
            }

            await _providerRepository.Update(provider);
        }

        public async Task UpdateAddress(Address address)
        {
            if (!ExecuteValidation(new AddressValidation(), address))
                return;

            await _addressRepository.Update(address);
        }
    }
}
