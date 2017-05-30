namespace Link.Domain.Contracts
{
    public interface IPackageValidator
    {
        bool IsValidId(string packageId);
    }
}
