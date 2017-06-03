using Link.Domain.Entities;

namespace Link.Domain.Contracts
{
    public interface IAddressValidator
    {
        bool IsValid(Address address);
    }
}