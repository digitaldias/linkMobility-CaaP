using Link.Domain.Contracts;
using Link.Domain.Entities;
using System.Threading.Tasks;

namespace Link.Data.Rest
{
    public class PackageRepository : IPackageRepository
    {
        public Task<Package> GetPackageByIdAsync(string packageId)
        {
            return Task.FromResult(new Package
            {
                Id = "yes"
            });
        }
    }
}
