using Link.Domain.Contracts;
using System;
using System.Linq;

namespace Link.Business
{
    public class PackageValidator : IPackageValidator
    {
        private const int MIN_DIGITS_IN_PACKAGEID     = 10;
        private const int MAX_DIGITS_IN_PACKAGEID     = 16;
        private const int MIN_CHARACTERS_IN_PACKAGEID = 13;
        private const int MAX_CHARACTERS_IN_PACKAGEID = 20;


        public bool IsValidId(string packageId)
        {
            if (string.IsNullOrEmpty(packageId))
                return false;

            int characterCount = packageId.Length;

            if (characterCount < MIN_CHARACTERS_IN_PACKAGEID)
                return false;

            if (characterCount > MAX_CHARACTERS_IN_PACKAGEID)
                return false;

            var digitsInString = packageId.Count(c => Char.IsNumber(c));

            if (digitsInString < MIN_DIGITS_IN_PACKAGEID)
                return false;

            if (digitsInString > MAX_DIGITS_IN_PACKAGEID)
                return false;

            return true;
        }
    }
}
