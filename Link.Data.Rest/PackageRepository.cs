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
            return Task.FromResult(new Package
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
                }
            });
        }
    }
}
