using Link.Domain.Contracts;
using Link.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Business
{
    public class PackageManager : IPackageManager
    {
        private readonly IPackageValidator _packageValidator;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IPackageRepository _packageRepository;

        public PackageManager(IPackageValidator packageValidator, IExceptionHandler exceptionHandler, IPackageRepository packageRepository)
        {
            _packageValidator = packageValidator;
            _exceptionHandler = exceptionHandler;
            _packageRepository = packageRepository;
        }


        public async Task<Package> RetrievePackageInfoAsync(string packageId)
        {
            if (!_packageValidator.IsValidId(packageId))
                return null;

            return await _exceptionHandler.GetAsync( () => _packageRepository.GetPackageByIdAsync(packageId));
        }


        public Task SetDeliveryAddressAsync(string packageId, Address deliveryAddress)
        {
            throw new NotImplementedException();
        }
    }
}
