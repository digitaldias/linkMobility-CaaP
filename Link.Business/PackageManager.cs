using Link.Domain.Contracts;
using Link.Domain.Entities;
using System.Threading.Tasks;

namespace Link.Business
{
    public class PackageManager : IPackageManager
    {
        private readonly IPackageValidator _packageValidator;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IPackageRepository _packageRepository;
        private readonly IAddressValidator _addressValidator;

        public PackageManager(IPackageValidator packageValidator, IExceptionHandler exceptionHandler, IPackageRepository packageRepository, IAddressValidator addressValidator)
        {
            _packageValidator = packageValidator;
            _addressValidator = addressValidator;
            _exceptionHandler = exceptionHandler;
            _packageRepository = packageRepository;
        }


        public async Task<Package> RetrievePackageInfoAsync(string packageId)
        {
            if (!_packageValidator.IsValidId(packageId))
                return null;

            return await _exceptionHandler.GetAsync( () => _packageRepository.GetPackageByIdAsync(packageId));
        }


        public async Task<Package> SetDeliveryAddressAsync(string packageId, Address deliveryAddress)
        {
            if (!_packageValidator.IsValidId(packageId))
                return null;

            if (!_addressValidator.IsValid(deliveryAddress))
                return null;

            return await _exceptionHandler.GetAsync(() => _packageRepository.SetNewDeliveryAddressAsync(packageId, deliveryAddress));
        }
    }
}
