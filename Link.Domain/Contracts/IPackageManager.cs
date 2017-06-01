using Link.Domain.Entities;
using System.Threading.Tasks;

namespace Link.Domain.Contracts
{
    public interface IPackageManager
    {
        Task<Package> RetrievePackageInfoAsync(string packageId);


        Task SetDeliveryAddressAsync(string packageId, Address deliveryAddress);
    }
}
