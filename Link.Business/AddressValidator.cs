using Link.Domain.Contracts;
using Link.Domain.Entities;

namespace Link.Business
{
    public class AddressValidator : IAddressValidator
    {
        public bool IsValid(Address address)
        {
            if (address == null)
                return false;

            if (string.IsNullOrEmpty(address.StreetAddress))
                return false;

            if (string.IsNullOrEmpty(address.ZipCode))
                return false;

            if (string.IsNullOrEmpty(address.City))
                return false;

            if (string.IsNullOrEmpty(address.Country))
                return false;

            return true;
        }
    }
}
