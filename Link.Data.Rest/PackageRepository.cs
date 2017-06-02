using Link.Domain.Contracts;
using Link.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Link.Data.Rest
{
    public class PackageRepository : IPackageRepository
    {
        public Task<Package> GetPackageByIdAsync(string packageId)
        {
            var package = new Package
            {
                Id                   = packageId,
                Status               = "Waiting in Transit",
                ExpectedDeliveryDate = DateTime.Now.AddDays(2),
                ShipmentDate         = DateTime.Now.AddDays(-1),
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

            return Task.FromResult(package);
        }
    }
}
