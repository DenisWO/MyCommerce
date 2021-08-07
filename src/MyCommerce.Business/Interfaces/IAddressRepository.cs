using MyCommerce.Business.Models;
using System;
using System.Threading.Tasks;

namespace MyCommerce.Business.Interfaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address> GetAddressByProvider(Guid providerId);
    }
}
