using Link.Domain.Contracts;
using Link.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Link.Data.Rest
{
    public class PackageRepository : IPackageRepository
    {
        public async Task<Package> GetPackageByIdAsync(string packageId)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            var package = new Package
            {
                Id                   = packageId,
                Status               = "Waiting in Transit",
                ExpectedDeliveryDate = DateTime.Now.AddDays(2),
                ShipmentDate         = DateTime.Now.AddDays(-1),
                CurrentLocation      = "Peru, Lima",
                Weight               = 3,
                WeightUnit           = "Kg",
                Dimensions           = new Dimensions
                {
                    Unit   = "cm",
                    Length = 10,
                    Width  = 30,
                    Height = 20
                },
                DeliveryAddress = new Address {
                    Country = "Sweden",
                    City = "Malmö",
                    ZipCode = "123 123",
                    StreetAddress = "Citadellvägen 13"
                }
            };
            package.ExpectedDeliveryDate = package.ExpectedDeliveryDate.Date + new TimeSpan(16, 0, 0);

            return package;
        }

        public async Task<Package> SetNewDeliveryAddressAsync(string packageId, Address deliveryAddress)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            var package = await GetPackageByIdAsync(packageId);

            if (package == null)
                return null;

            package.DeliveryAddress = deliveryAddress;

            return package;
        }
    }
}
