using Link.Domain.Contracts;
using System;

namespace Link.Business
{
    public class PackageValidator : IPackageValidator
    {
        public bool IsValidId(string packageId)
        {
            if (string.IsNullOrEmpty(packageId))
                return false;

            return true;
        }
    }
}
