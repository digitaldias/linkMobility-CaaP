using Link.Domain.Contracts;
using System;

namespace Link.Business
{
    public class PackageValidator : IPackageValidator
    {
        private const int MIN_CHARACTERS_IN_PACKAGEID = 10;
        private const int MAX_CHARACTERS_IN_PACKAGEID = 16;

        public bool IsValidId(string packageId)
        {
            if (string.IsNullOrEmpty(packageId))
                return false;

            if (packageId.Length < MIN_CHARACTERS_IN_PACKAGEID)
                return false;

            if (packageId.Length > MAX_CHARACTERS_IN_PACKAGEID)
                return false;

            return true;
        }
    }
}
