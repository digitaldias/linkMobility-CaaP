using Link.Domain.Entities;
using System.Threading.Tasks;

namespace Link.Domain.Contracts
{
    public interface IPackageRepository
    {
        Task<Package> GetPackageByIdAsync(string packageId);
    }
}
